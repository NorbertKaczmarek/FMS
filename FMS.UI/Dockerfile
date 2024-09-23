FROM node:latest AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build 

FROM alpine:latest
RUN mkdir /app-output
COPY --from=build /app/dist/fms.ui /app-output
CMD ["cp", "-r", "/app-output/browser/.", "/data/fms.ui"]
