# Stage 1

FROM node:10-alpine as build-step

RUN mkdir -p /app
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY AngularUI/PMMSAngular.AngularUI/package.json /app
RUN npm install

COPY AngularUI/PMMSAngular.AngularUI/. /app

#RUN npm cache clean --force
#RUN rm -rf node_modules
#all
#RUN npm install -g @angular/cli@7.1.3 --unsafe

RUN npm run prodbuild

# Stage 2

FROM nginx:1.17.1-alpine
COPY --from=build-step /app/dist /usr/share/nginx/html
