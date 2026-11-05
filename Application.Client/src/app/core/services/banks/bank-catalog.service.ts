import { Injectable } from '@angular/core';

// Public, version-pinned assets/datasets from https://github.com/rzmt/logos-bancos-br.
// Use native fetch rather than HttpClient: Money's global interceptor adds a private
// Authorization header to HttpClient requests, which must never reach the public CDN.
const CATALOG_CDN = 'https://cdn.jsdelivr.net/npm/logos-bancos-br@0.8.4';
const SAFE_LOGO_PATH = /^logos\/(?:png|svg)\/\d{8}\.(?:png|svg)$/;

export interface BankInstitution {
  ispb: string;
  compe: string | null;
  compe4: string | null;
  name: string;
  shortName: string;
  displayName: string | null;
  logo: { png: string; svg: string | null } | null;
}

interface BanksDataset {
  banks: BankInstitution[];
}

interface PixDataset {
  institutions: BankInstitution[];
}

function normalize(value: string): string {
  return value.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase().trim();
}

export function institutionName(institution: BankInstitution): string {
  return institution.displayName || institution.shortName || institution.name;
}

@Injectable({ providedIn: 'root' })
export class BankCatalogService {
  private institutions: BankInstitution[] = [];
  private byIspb = new Map<string, BankInstitution>();
  private pending?: Promise<readonly BankInstitution[]>;

  load(): Promise<readonly BankInstitution[]> {
    if (!this.pending) {
      this.pending = this.fetchCatalog().catch(error => {
        this.pending = undefined; // allow a retry after a transient CDN failure
        throw error;
      });
    }
    return this.pending;
  }

  get(ispb: string | null | undefined): BankInstitution | undefined {
    return ispb ? this.byIspb.get(ispb) : undefined;
  }

  logoUrl(institution: BankInstitution | null | undefined): string | null {
    const path = institution?.logo?.svg || institution?.logo?.png;
    return path && SAFE_LOGO_PATH.test(path) ? `${CATALOG_CDN}/${path}` : null;
  }

  find(query: string, limit = 40): BankInstitution[] {
    const normalized = normalize(query);
    if (!normalized) {
      // The package curates popular display names. Prefer those for the empty search.
      return this.institutions
        .filter(bank => !!bank.displayName && !!bank.logo)
        .sort((a, b) => institutionName(a).localeCompare(institutionName(b), 'pt-BR'))
        .slice(0, 30);
    }

    const terms = normalized.split(/\s+/);
    return this.institutions
      .filter(bank => {
        const searchable = normalize([
          bank.displayName || '', bank.name, bank.shortName || '', bank.compe || '',
          bank.compe4 || '', bank.ispb,
        ].join(' '));
        return terms.every(term => searchable.includes(term));
      })
      .sort((a, b) => {
        const aName = normalize(institutionName(a));
        const bName = normalize(institutionName(b));
        const aRank = (aName === normalized ? 0 : aName.startsWith(normalized) ? 1 : 2);
        const bRank = (bName === normalized ? 0 : bName.startsWith(normalized) ? 1 : 2);
        if (aRank !== bRank) return aRank - bRank;
        if (!!a.logo !== !!b.logo) return a.logo ? -1 : 1;
        return institutionName(a).localeCompare(institutionName(b), 'pt-BR');
      })
      .slice(0, limit);
  }

  private async fetchCatalog(): Promise<readonly BankInstitution[]> {
    // credentials: omit ensures no Money cookies/credentials go to the third-party CDN.
    const options: RequestInit = { credentials: 'omit', cache: 'force-cache' };
    const [banksResponse, pixResponse] = await Promise.all([
      fetch(`${CATALOG_CDN}/data/bancos.json`, options),
      fetch(`${CATALOG_CDN}/data/instituicoes-pix.json`, options),
    ]);
    if (!banksResponse.ok || !pixResponse.ok) {
      throw new Error('Failed to load the financial institutions catalog.');
    }
    const [banks, pix] = await Promise.all([
      banksResponse.json() as Promise<BanksDataset>,
      pixResponse.json() as Promise<PixDataset>,
    ]);
    if (!Array.isArray(banks.banks) || !Array.isArray(pix.institutions)) {
      throw new Error('Invalid financial institutions catalog.');
    }

    const institutions = new Map<string, BankInstitution>();
    for (const bank of [...banks.banks, ...pix.institutions]) {
      if (/^\d{8}$/.test(bank.ispb)) institutions.set(bank.ispb, bank);
    }
    this.institutions = [...institutions.values()];
    this.byIspb = institutions;
    return this.institutions;
  }
}
