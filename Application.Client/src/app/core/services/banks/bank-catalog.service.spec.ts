import { TestBed } from '@angular/core/testing';
import { BankCatalogService, BankInstitution } from './bank-catalog.service';

describe('BankCatalogService', () => {
  const bank: BankInstitution = {
    ispb: '18236120',
    compe: '260',
    compe4: '0260',
    name: 'NU PAGAMENTOS S.A.',
    shortName: 'NU PAGAMENTOS',
    displayName: 'Nubank',
    logo: { png: 'logos/png/18236120.png', svg: 'logos/svg/18236120.svg' },
  };
  const pix: BankInstitution = {
    ispb: '12345678',
    compe: null,
    compe4: null,
    name: 'FINTECH TESTE',
    shortName: 'FINTECH TESTE',
    displayName: null,
    logo: null,
  };

  let service: BankCatalogService;
  let fetchSpy: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BankCatalogService);
    fetchSpy = spyOn(window, 'fetch').and.callFake(async (input: RequestInfo | URL) => {
      const payload = String(input).includes('instituicoes-pix')
        ? { institutions: [pix] }
        : { banks: [bank] };
      return new Response(JSON.stringify(payload), {
        status: 200,
        headers: { 'Content-Type': 'application/json' },
      });
    });
  });

  it('loads both collections once, without forwarding credentials to the CDN', async () => {
    const first = await service.load();
    const second = await service.load();

    expect(first).toBe(second);
    expect(first.length).toBe(2);
    expect(fetchSpy).toHaveBeenCalledTimes(2);
    expect(fetchSpy.calls.first().args[1]).toEqual(jasmine.objectContaining({
      credentials: 'omit',
    }));
  });

  it('searches by popular name, official name, COMPE and ISPB', async () => {
    await service.load();

    expect(service.find('Núbánk')[0].ispb).toBe('18236120');
    expect(service.find('PAGAMENTOS')[0].ispb).toBe('18236120');
    expect(service.find('0260')[0].ispb).toBe('18236120');
    expect(service.find('18236120')[0].ispb).toBe('18236120');
    expect(service.find('fintech')[0].ispb).toBe('12345678');
    expect(service.get('12345678')?.name).toBe('FINTECH TESTE');
  });

  it('builds pinned CDN URLs and handles missing or unexpected paths', async () => {
    await service.load();

    expect(service.logoUrl(bank)).toBe(
      'https://cdn.jsdelivr.net/npm/logos-bancos-br@0.8.4/logos/svg/18236120.svg'
    );
    expect(service.logoUrl(pix)).toBeNull();
    expect(service.logoUrl({
      ...bank,
      logo: { png: '../assets/logo.png', svg: null },
    })).toBeNull();
  });
});
