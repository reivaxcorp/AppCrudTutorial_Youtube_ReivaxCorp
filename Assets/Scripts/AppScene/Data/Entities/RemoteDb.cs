/*********************************************************************************
 * Nombre del Archivo:     RemoteDb.cs
 * Descripci�n:            Creamos, leemos, actualizamos, borramos los �tems en Realtime Database.
                           Tambi�n aprovecharemos la base de datos en tiempo real, al escuchar alg�n cambio
                           que ser� manejado por nuestro ItemManager.cs 
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

using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RemoteDb : IRepositoryRemote
{
    public delegate void OnHandleValueChangedCallBack(List<ItemRemote> itemsRemoteList);
    public event OnHandleValueChangedCallBack handleValueResult;
    private string userUid;


    public void SetUserUid(string userUid)
    {
        this.userUid = userUid;
    }

    public RemoteDb GetRemoteDb()
    {
        return this;
    }

    public async Task FirebaseValueChanged()
    {

        if (!IsUserUid()) return;

        FirebaseSDK.GetInstance().defaultInstance
            .GetReference("users")
            .Child(userUid)
            .Child("items")
            .ValueChanged += HandleValueChanged;

        // Esperar 1 segundo antes de continuar para asegurarse de que el suscriptor se ha registrado correctamente
        await Task.Delay(1000);
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        // Do something with the data in args.Snapshot

        List<ItemRemote> itemsRemoteList = new List<ItemRemote>();

        foreach (DataSnapshot itemSnapshot in args.Snapshot.Children)
        {
            // Obtener el valor del DataSnapshot y convertirlo a un diccionario
            Dictionary<string, object> itemData = (Dictionary<string, object>)itemSnapshot.Value;

            // Crear un nuevo objeto ItemRemote y asignar los valores del diccionario
            ItemRemote item = new ItemRemote
            {
                // Ajusta estas l�neas seg�n la estructura de tus datos remotos
                Id = itemData["id"].ToString(),
                Name = itemData["name"].ToString(),
                ImageName = itemData["image_name"].ToString(),
                CreationDate = long.Parse(itemData["creation_date"].ToString())
            };

            // Agregar el objeto ItemRemote a la lista
            itemsRemoteList.Add(item);
        }

        AppConfig.SetCurrentItemsCount(itemsRemoteList.Count);
        // Debug.Log("handled itemsRemoteList " + itemsRemoteList.Count);
        handleValueResult?.Invoke(itemsRemoteList);
    }

    public void CancelHandleValueChanged()
    {
        if (!IsUserUid()) return;

        FirebaseSDK.GetInstance().defaultInstance
         .GetReference("users")
         .Child(userUid)
         .Child("items")
         .ValueChanged -= HandleValueChanged; // unsubscribe from ValueChanged.
    }


    public async Task<bool> SaveItemRemote(ItemRemote itemRemote, IResult resultUi)
    {
        bool saveSuccess = false;

        if (!IsUserUid()) return saveSuccess;

        DatabaseReference rootRef = FirebaseSDK.GetInstance().defaultInstance.RootReference;

        // key generada con Push()
        string key = rootRef.Child("users").Child(userUid).Child("items").Push().Key;

        // Obtener la marca de tiempo del servidor en formato Unix
        long timestampUnix = TimeUtils.GetTimeStampUnix();

        ItemRemote entry =
            new ItemRemote(key, itemRemote.Name, itemRemote.ImageName, timestampUnix);

        Dictionary<string, System.Object> entryValues = entry.ToDictionary();

        await rootRef.Child("users").Child(userUid).Child("items").Child(key).SetValueAsync(entryValues)
            .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Manejar error
                Debug.LogError("Error al escribir en la base de datos: " + task.Exception);
                resultUi.SetResultCrudUi("Error", "Error al escribir en la base de datos");
                saveSuccess = false;
            }
            else
            {
                // Operaci�n exitosa
                Debug.Log("Datos escritos exitosamente en la base de datos");
                resultUi.SetResultCrudUi("Completado", "Nuevo �tem agregado");
                saveSuccess = true;
            }
        });

        return saveSuccess;
    }

    public async Task<bool> UpdateItemRemote(ItemRemote itemRemote, IResult iResult)
    {
        bool updateSuccess = false;

        if (!IsUserUid()) return updateSuccess;

        DatabaseReference rootRef = FirebaseSDK.GetInstance().defaultInstance.RootReference;
        await rootRef
             .Child("users")
             .Child(userUid)
             .Child("items")
             .Child(itemRemote.Id)
             .UpdateChildrenAsync(itemRemote.ToDictionary()).ContinueWithOnMainThread(task =>
             {
                 if (task.IsFaulted || task.IsCanceled)
                 {

                     // Manejar error
                     Debug.LogError("Error al escribir en la base de datos: " + task.Exception);
                     iResult.SetResultCrudUi("Error", "Error al actualizar");
                     updateSuccess = false;
                 }
                 else
                 {
                     // Operaci�n exitosa
                     Debug.Log("Datos escritos exitosamente en la base de datos");
                     iResult.SetResultCrudUi("Actualizaci�n", "Datos actualizados exitosamente");
                     updateSuccess = true;
                 }
             });

        return updateSuccess;
    }

    public async Task<bool> DeleteItemRemoteById(string id, IResult iResult)
    {
        if (!IsUserUid()) return false;

        DatabaseReference rootRef = FirebaseSDK.GetInstance().defaultInstance.RootReference;

        bool deleteSuccess = false;

        await rootRef
             .Child("users")
             .Child(userUid)
             .Child("items")
             .Child(id)
             .RemoveValueAsync().ContinueWithOnMainThread(task =>
             {
                 if (task.IsFaulted || task.IsCanceled)
                 {

                     // Manejar error
                     Debug.LogError("Error al borrar el item remoto en la base de datos: " + task.Exception);
                     iResult.SetResultCrudUi("Error", "Error al borrar el �tem remoto de la base de datos");
                     deleteSuccess = false;
                 }
                 else
                 {
                     // Operaci�n exitosa
                     Debug.Log("�tem remoto borrado correctamente");
                     iResult.SetResultCrudUi("Borrado", "�tem remoto borrado correctamente");
                     deleteSuccess = true;
                 }
             });
        return deleteSuccess;
    }

    private bool IsUserUid()
    {
        if (userUid == null)
        {
            Debug.LogWarning("No hay un userUid!!!");
            return false;
        }
        return true;
    }
}