# Como desplegar un contenedor en Docker desktop
1. Instalar Docker Desktop desde https://www.docker.com/products/docker-desktop
2. Abrir Docker Desktop y asegurarse de que está corriendo.
3. crear un archivo Dockerfile en el directorio raíz de tu proyecto con el siguiente contenido:
4. no es indispensable pero recomendable crear un archivo Docker-compose.yml en el directorio raíz de tu proyecto.
5. correr el comando en la terminal:
```bash
docker-compose up --build
```


# Paso a paso para desplegar en Docker + Kubernetes en local
Este es un tutorial paso a paso para desplegar una aplicación en Docker y Kubernetes de forma manual y utilizando Skaffold en un entorno local. 
Skaffold facilita el desarrollo continuo de aplicaciones en Kubernetes al automatizar el proceso de construcción, despliegue y prueba.

## Requisitos para la ejecucion manual
1. Configurar el archivo deployment.yaml con la imagen de Docker creada.
2. Configurar el archivo service.yaml con el puerto que se desea exponer.
3. en la terminal correr los siguientes comandos:
```bash
kubectl apply -f .\catalog-deployment.yaml
kubectl apply -f .\catalog-service.yaml
# o juntos:
kubectl apply -f .\catalog-deployment.yaml -f .\catalog-service.yaml

```
4. Comprobar que el deployment y el servicio se han creado correctamente:
```bash
kubectl get pods -n catalog
kubectl get deploy -n catalog
kubectl get svc -n catalog
kubectl logs -n catalog deployment/<nombre-deployment>   # o kubectl logs pod/<pod-name>
kubectl describe pod <pod-name> -n catalog

```
5. Acceder a la aplicacion en la ruta ***http://localhost:30080/***.


## Requisitos previos con Skaffold
1. Skaffold version
2. Configurar el archivo skaffold.yaml con la imagen de Docker creada, el namespace, y los archivos de deployment y service.
3. Skaffold dev
4. Skaffold run

Accede a la aplicacion en la ruta ***http://localhost:30080/***.

### Obtener deployments en kubernestes
```bash	
kubectl get deployments -n <namespace>
```
### Eliminar un deployment en kubernestes
```bash
kubectl delete deployment <nombre-del-deployment> -n <namespace>
```

### Obtener servicios en kubernestes
```bash
kubectl get svc -n <namespace>
```

### Eliminar un servicio en kubernestes
```bash	
kubectl delete service <nombre-del-service> -n <namespace>
```

### Obtener pods en kubernestes
```bash
kubectl get pods -n <namespace>
```

### Eliminar un servicio en kubernestes
```bash	
kubectl delete pods <nombre-del-pod> -n <namespace>
```
