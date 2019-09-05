# Stage 0, build application using Node.js environment
FROM node:10 as build-stage

WORKDIR /app

COPY src/Web/TemplateManagementApp/ClientApp/ /app/
RUN npm install
RUN npm run build -- --output-path=./dist/out --prod

# Stage 1, based on Nginx, to have only the compiled app, ready for production with Nginx
FROM nginx:1.15
COPY --from=build-stage /app/dist/out/ /usr/share/nginx/html
COPY src/Web/TemplateManagementApp/ClientApp/nginx-custom.conf /etc/nginx/conf.d/default.conf