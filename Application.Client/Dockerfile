FROM node:20-alpine AS build
WORKDIR /app

COPY Application.Client/package*.json ./
RUN npm ci

COPY Application.Client/ ./
RUN npm run build -- --configuration production

FROM nginx:1.27-alpine AS final
RUN rm -rf /usr/share/nginx/html/*
COPY Application.Client/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/application.client/browser/ /usr/share/nginx/html/
RUN cp /usr/share/nginx/html/index.csr.html /usr/share/nginx/html/index.html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
