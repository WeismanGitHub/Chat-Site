FROM node:alpine

EXPOSE 5001/tcp

WORKDIR /usr/src/app
WORKDIR /usr/src/app/Source/Client

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

WORKDIR /usr/src/app