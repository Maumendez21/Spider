### Stage 1: build ###

FROM node:12.7-alpine AS build
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build --prod

## Stage 2: Run ###

FROM nginx:1.17.1-alpine AS prod-stage
COPY --from=build /app/dist/SpiderFleet /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
