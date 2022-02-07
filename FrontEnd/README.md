![](http://spiderfleetapi.azurewebsites.net/assets/images/logospider.png)
# Comienzo
Spider Fleet es sistema inteligente para monitorear el rendimiento, la ubicación e indicadores clave de tus vehículos.

## Instalación 
####Angular CLI
Lo primero que se necesita para empezar a trabajar en la aplicación  es necesario instalar Angular CLII globalmente en su equipo.

`npm install -g @angular/cli`


####Clonar

`$ git clone https://MMAGICA@dev.azure.com/MMAGICA/SPIDER%20FLEET/_git/SPIDER%20FLEET`

####Instalación de dependencias

Navegar a la carpeta FrontEnd y hacer la instalación de las depencias del proyecto

`1. cd ~\SPIDER%20FLEET\FrontEnd`
`2. npm install`

#### Enviroments
Antes de correr la aplicación es necesario crear dos arhivos que contienen las credenciales de las API Keys, que sos consumidas por la aplicación.
* En la capeta del proyecto **src** crear una carpeta llamada *environments*
* Crear dos nuevos archivos dentro de esa nueva carpeta.
	El primero llamado: **environment.prod.ts**
	El segundo llamado: ** environment.ts**
* Posteriormente poner la misma estructura en los dos archivos que se muestra acontinuación: 
```javascript
export const environment = {
    production: boolean,
    apiKey: 'apiKey',
    libraries: []
};
```
* Una vez realizado todo lo indicado, reiniciar el editor de texto.


####  Correr Aplicación 
Para correr la aplicación ejectuar: 

`$ ng serve -o`

### Generar build para Docker

```docker
docker build -t spiderdocker .
```

### Levantar Aplicación en Docker

```docker
docker run -d -it -p 8000:80 --name spiderdocker spiderdocker
```
