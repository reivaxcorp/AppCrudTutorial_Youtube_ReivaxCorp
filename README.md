# App Crud Tutorial Unity 3D

Este proyecto es una aplicación CRUD (Create, Read, Update, Delete) desarrollada en Unity 3D. La aplicación utiliza Firebase para gestionar datos en tiempo real, almacenamiento de archivos y autenticación.
La aplicación trata específicamente en el manejo de texturas dentro en una aplicación Android.
A continuación, se detallan los pasos para configurar y ejecutar la aplicación:

## Videos tutoriales del proyecto

La serie de tutoriales consta de varios videos, cada uno cubriendo un aspecto específico del trabajo con Firebase y texturas en Unity. Puedes encontrar la lista de reproducción completa [aquí](https://youtube.com/playlist?list=PLsvltDspdJcfiiWy2baA2MCNzBm32USjv&si=q7dTsZltYs-d3eOI).

## Configuración en Firebase

- Crea un proyecto en Firebase:
- Deshabilita Analytics para este proyecto, al crear el proyecto.
- Ve a Descripción general del proyecto -> Configuración del proyecto.
- En "Tu apps", crea un nuevo proyecto y selecciona la plataforma (en nuestro caso, Android).
-	En la pestaña “Agrega Firebase a tu app para Android”, ingresa el nombre de tu paquete, que utilizarás también en Unity 3D.
-	Luego selecciona a registrar app.
-	Termina el resto del registro con "Siguiente".
- Ve a la consola, "Ir a consola"

# Configura las reglas de firebase Realtime Database:
- En la barra lateral, en Compilación, selecciona "Realtime Database"
- Luego le selecciona a "Crear una base de datos"
- Selecciona la ubicación que más te convenga.
- Luego, en "Configurar base de datos", tilda "Comenzar e nmodo de prueba", despúes lo modificamos.
-	Despúes, en la sección “Realtime Database”, establece las reglas de acceso, el la pestaña "Reglas".
- Copia y pega estas reglas:

  ```json
{
"rules": {      
     "users": {
      "$userUid": {
       	"items": {
           ".read": "auth != null && $userUid === auth.uid && auth.token.email_verified == true",
           ".write": "auth != null && $userUid === auth.uid && auth.token.email_verified == true",
            "name": {".validate": "newData.isString() && newData.val().length <= 30"}
      	}
      }
    }
  }
}
  ```
# Configura las reglas de firebase Realtime Database:
- En la barra lateral, en Compilación, selecciona "Storage"
- Luego selecciona "Comenzar"
- Luego en Configura Cloud Storage, selecciona "Comenzar en modo de prueba", luego lo modificamos.
- Selecciona la zona que mejor te convenga, y selecciona "Listo".
-	Despúes, en la sección “Storage”, establece las reglas de acceso, el la pestaña "Reglas".
- Copia y pega estas reglas:

```markdown
rules_version = '2';

service firebase.storage {
  match /b/{bucket}/o {
    match /users/{uidFolder}/imageItems/{fileName} {
      allow write: if request.auth.uid != null
      						 && request.auth.uid == uidFolder
                   && request.auth.token.email_verified == true
                   && request.resource.size < 5 * 1024 * 1024;
      allow read: if request.auth.uid != null
      						&& request.auth.uid == uidFolder
                  && request.auth.token.email_verified == true;
      allow delete:  if request.auth.uid != null
      						&& request.auth.uid == uidFolder
                  && request.auth.token.email_verified == true;
                   
    }
  }
}
```
# Configura la autenticación:
- En la barra lateral, en Compilación, selecciona "Authentication", habilita el método de acceso con “Correo electrónico/contraseña”.
- Luego selecciona "Comenzar"
- Despúes en "Agrega tu primer método de acceso y comienza a utilizar Firebase Auth", selecciona "Correo electrónico/contraseña"
- En Proveedores de acceso, habilita la opcion Correo electrónico/contraseña, y selecciona "guardar"
- El siguiente paso, en la pestaña "Plantillas" personaliza las plantillas de confirmación de email, que se enviara a los usuarios, cuando inicien sesión.
- Selecciona editar plantilla, y en nombre de remitente puedes poner el nombre de tu aplicación, y si quieres cambias el idioma. Luego selecciona guardar.
# Descargar la configuración:
- Luego debes descargar el google-services.json. Recuerda que si añades otra funcionalidad, tambien deberas volver a descargar este archivo actualizado.
- Te vas a Descripción general del proyecto -> Configuración de proyecto.
- En "Tus apps", descarga el "google-services.json"
  
## Configuración en Unity 3D para Firebase

  # Descarga el SDK de Firebase:
  - Sigue la documentación oficial y descarga el SDK de Firebase desde la página oficial: Descargar Firebase para Unity.
    [SDK firebase](https://firebase.google.com/download/unity?hl=es-419)
  # Instala los paquetes necesarios:
  - Importa los siguientes paquetes al proyecto:
        FirebaseAuth.unitypackage,
        FirebaseDatabase.unitypackage,
        FirebaseStorage.unitypackage
  # Agrega el archivo google-services.json:
  - Coloca el archivo google-services.json en la carpeta “Assets” de tu proyecto Unity 3D.
    Este archivo contiene la configuración específica de tu proyecto Firebase.
  # Configura el nombre del paquete:
  - En Unity, en Edit -> Project Settings... -> Player,  en la pestaña “Identification”,
    asegúrate de que el “Package name” coincida con el nombre de paquete que configuraste en Firebase. Esto es importante para que Firebase funcione correctamente.

  # Configuración de la activity para la selección de archivos en Android
  - En la carpeta “Assets”, encontrarás el archivo CrudUnityPlayerActivity.java.
  - Asegúrate de que el nombre del paquete en este archivo sea el mismo que el configurado en Firebase.
  - Luego, en la carpeta “Assets/Plugins/Android”, encontrarás el archivo AndroidManifest.xml.
    Asegúrate de que este archivo contenga los permisos que el usuario deberar confirmar para acceder al almacenamiento de su dispositivo.
  - AndroidManifest.xml
   ```xml
        <?xml version="1.0" encoding="utf-8"?>
        <!-- GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN-->
        <manifest
            xmlns:android="http://schemas.android.com/apk/res/android"
            package="com.yourcompany.unityappcrud"
            xmlns:tools="http://schemas.android.com/tools">
        	<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
        	<uses-permission android:name="android.permission.INTERNET" />
        	<uses-permission android:name="com.google.android.gms.permission.AD_ID"/>
        	
        	<application>
                <activity android:name="com.yourcompany.unityappcrud.UnityPlayerActivity"
                          android:theme="@style/UnityThemeSelector">
                    <intent-filter>
                        <action android:name="android.intent.action.MAIN" />
                        <category android:name="android.intent.category.LAUNCHER" />
                    </intent-filter>
                    <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
                </activity>
            </application>
        </manifest>
  ```
 - En “com.youtcompany.unityappcrud” debes reemplazar con tu nombre de paquete.

# Habilita las opciones de desarrollador (Emulador - Dispositivo físico):
- En tu dispositivo Android, ve a la configuración y busca la sección “Acerca del teléfono” o “Información del software”.
- Busca el número de compilación y tócalo varias veces hasta que se active el modo de desarrollador.
- Luego, en la configuración, verás una nueva opción llamada “Opciones de desarrollador”. Habilita la “Depuración USB”.
# Configura una keystore:
- Antes de ejecutar la aplicación en un emulador o dispositovo, debes crear una keystore (almacén de claves) para firmar tu APK.
- Si ya tienes una keystore, selecciona la que deseas usar. Si no, crea una nueva. Recuerda que es necesario para poder subir tu app a
  la Play Store.
# Rutas de almacenamiento en el editor:
- Cuando pruebas la aplicación en el editor de Unity, las imágenes y datos se guardarán en una ubicación específica.
- En Windows, la ruta será similar a: C:\Users\nombredeusuario\AppData\LocalLow\company_name\product_name.
- Puedes modificar estos nombres en la configuración del proyecto en Unity (Project Settings > Player).
# Recursos adicionales:
- Si necesitas más detalles o instrucciones específicas, consulta la lista de reproducción en mi tutorial en YouTube.
- Lista de Reproducción: [Aplicación CRUD en Unity 3D, utilizando Firebase.](https://www.youtube.com/playlist?list=PLsvltDspdJcfiiWy2baA2MCNzBm32USjv)

## Uso del Código
- Derechos de Autor © [2024] ReivaxCorp
- Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de documentación asociados (el “Software”), para tratar con el Software sin restricciones, incluyendo, pero no limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las siguientes condiciones:
- El aviso de derechos de autor anterior y este aviso de permiso se incluirán en todas las copias o partes sustanciales del Software realizadas por el desarrollador, específicamente en las carpetas “Assets/Scripts”.
- Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D están sujetas a los derechos de autor de los respectivos desarrolladores o artistas, así como a las políticas de Unity 3D.
- EL SOFTWARE SE PROPORCIONA “TAL CUAL”, SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, INCLUYENDO, PERO NO LIMITADO A, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN PROPÓSITO PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DE DERECHOS DE AUTOR SERÁN RESPONSABLES DE CUALQUIER RECLAMACIÓN, DAÑO U OTRA RESPONSABILIDAD, YA SEA EN UNA ACCIÓN DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXIÓN CON EL SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.

## Conéctate Conmigo

Si tienes alguna pregunta, comentario o sugerencia, no dudes en ponerte en contacto:

- Correo electrónico: [reivaxcorp@email.com](mailto:reivaxcorp@gmail.com)
- LinkedIn: [Javier Monzón](https://www.linkedin.com/in/javier-monzón-a527952b5)
- YouTube: [ReivaxCorp.](https://www.youtube.com/channel/UCFaeV4z3zCTvF48ay6q7MtQ)
- Lista de Reproducción: [Aplicación CRUD en Unity 3D, utilizando Firebase.](https://www.youtube.com/playlist?list=PLsvltDspdJcfiiWy2baA2MCNzBm32USjv)
- Google Play: [ReivaxCorp](https://play.google.com/store/apps/dev?id=6165909766232622777)
- Sitio Web: [reivaxcorp.com](https://reivaxcorp.com)

 ¡Saludos!,
 Javier.
