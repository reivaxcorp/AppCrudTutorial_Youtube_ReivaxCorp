/*********************************************************************************
 * Nombre del Archivo:     ManageStorageRemote.cs
 * Descripci�n:            Clase encargada de bajar y borrar los archivos de Firebase Storage.  
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor � [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentaci�n asociados (el �Software�), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, espec�ficamente en las carpetas �Assets/Scripts�.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D est�n sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, as� como a las pol�ticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA �TAL CUAL�, SIN GARANT�A DE NING�N TIPO, EXPRESA O IMPL�CITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANT�AS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SER�N RESPONSABLES DE CUALQUIER RECLAMACI�N, DA�O U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCI�N DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXI�N CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using Firebase.Extensions;
using Firebase.Storage;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Lectura,subida y borrado de im�genes de firebase storage
/// </summary>
public class ManageStorageRemote
{
    private string _storageUrl = "gs://appcrudunity3d.appspot.com/users/"; // Reemplaza con la URI p�blica de tu imagen.
    private string _folderUserUid;
    private string _generateImageName;
    private byte[] _fileBytes;

    public ManageStorageRemote(string generateImageName, string folderUserUid, byte[] fileBytes)
    {
        _generateImageName = generateImageName;
        _folderUserUid = folderUserUid;
        _fileBytes = fileBytes;
    }

    public ManageStorageRemote(string imageName)
    {
        _storageUrl += FirebaseSDK.GetInstance().auth.CurrentUser.UserId + "/imageItems/" + imageName + ".png";
    }

    public async Task<bool> UploadFileFirebaseStorage()
    {
        FirebaseStorage firebaseStorage = FirebaseSDK.GetInstance().firebaseStorage;

        bool result = false;

        if (firebaseStorage != null)
        {

            if (_generateImageName != null)
            {

                StorageReference storageRef = firebaseStorage.GetReferenceFromUrl("gs://appcrudunity3d.appspot.com");
                StorageReference userRef = storageRef
                    .Child("users")
                    .Child(_folderUserUid)
                    .Child("imageItems")
                    .Child(_generateImageName + ".png");

                // Crear metadatos de archivo incluyendo el tipo de contenido
                var newMetadata = new MetadataChange();
                newMetadata.ContentType = "image/png";

                // Debemos continuar en el hilo principal, ya que debemos actualizar la UI, por eso usamos
                // ContinueWithOnMainThread.
                await userRef.PutBytesAsync(_fileBytes, newMetadata, null, CancellationToken.None)
                    .ContinueWithOnMainThread((Task<StorageMetadata> task) =>
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            Debug.Log(task.Exception.ToString());
                            result = false;
                        }
                        else
                        {
                            // Los metadatos contienen informaci�n del archivo como tama�o, tipo de contenido y hash md5.
                            StorageMetadata metadata = task.Result;
                            string md5Hash = metadata.Md5Hash;
                            Debug.Log("�Subida finalizada!" + metadata.Path);
                            Debug.Log("Hash MD5 = " + md5Hash);
                            result = true; // Se ha iniciado correctamente la operaci�n de subida
                        }
                    });
            }
            else
            {
                Debug.LogWarning("generateImageName es Null");
            }

            return result;
        }
        else
        {
            Debug.LogWarning("FirebaseStorage es null");
            return result; // No se ha podido iniciar la operaci�n de subida
        }
    }

    public async Task<Texture2D> DownloadImage()
    {
        TaskCompletionSource<Texture2D> initializationTask = new TaskCompletionSource<Texture2D>();

        // Parsea la URL de almacenamiento para obtener la referencia a la imagen.
        var storageReference = FirebaseSDK.GetInstance().firebaseStorage.GetReferenceFromUrl(_storageUrl);
        // Descarga el archivo.
        await storageReference.GetBytesAsync(long.MaxValue).ContinueWithOnMainThread(task2 =>
         {
             if (task2.IsFaulted || task2.IsCanceled)
             {
                 Debug.Log("La imag�n no existe " + _storageUrl);   
                 Debug.LogWarning("Error al descargar la imagen: " + task2.Exception);
                 initializationTask.SetResult(null);
             }
             else
             {
                 // Obtiene los bytes de la imagen descargada.
                 byte[] fileContents = task2.Result;

                 // Crea una textura desde los bytes de la imagen.
                 Texture2D texture = new Texture2D(1, 1);
                 bool loadImage = texture.LoadImage(fileContents);

                 if (loadImage)
                 {
                     initializationTask.SetResult(texture);
                 }
                 else
                 {
                     initializationTask.SetResult(null);
                     Debug.LogError("Error al cargar la textura desde los bytes.");
                 }
             }
         });

       return await initializationTask.Task;
    }

    /// <summary>
    /// Para actualizar y borrar, necesitamos borrar la imag�n anterior.
    /// </summary>
    /// <param name="filePath"></param>
    public async Task<bool> DeleteImageRemote()
    {
        Debug.Log("Imagen remota a eliminar: " + _storageUrl);

        var storageReference = FirebaseSDK.GetInstance().firebaseStorage.GetReferenceFromUrl(_storageUrl);

        bool deleteSuccess = false;

        await storageReference.DeleteAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Archivo remoto borrado correctamente.");
                deleteSuccess = true;
            }
            else
            {
                Debug.LogWarning("Archivo remoto anterior no encontrado.");
                deleteSuccess = false;
            }
        });

        return deleteSuccess;
    }
}
