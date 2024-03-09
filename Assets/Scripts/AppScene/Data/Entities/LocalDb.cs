/*********************************************************************************
 * Nombre del Archivo:     LocalDb.cs
 * Descripci�n:            Leeremos los datos locales almacenados en el dispositivo, asi tambi�n
 *                         los guardaremos.  
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class LocalDb : IRepositoryLocal
{
    private const string SAVE_FILE_NAME = "items.crud";
    private string folderNameUser;

    public void SetUserUidFolder(string folderNameUser)
    {
        this.folderNameUser = folderNameUser;
    }

    public async Task<ItemLocal> GetLocalItemById(string id)
    {
        List<ItemLocal> localItemsList = await GetLocalItemsAsync();

        int existingIndex = localItemsList.FindIndex(x => x.Id == id);

        if (existingIndex != -1)
        {
            return localItemsList[existingIndex];
        }
        throw new Exception("El item local fue borrado o no existe");
    }

    /// <summary>
    /// Salvamos los datos de los items en {userUid} en el dispositovo
    /// </summary>
    public async Task<List<ItemLocal>> GetLocalItemsAsync()
    {
        if (!IsUserFolderNameUid()) return new List<ItemLocal>();


        string folderPath = Path.Combine(Application.persistentDataPath, folderNameUser);
        string filePath = Path.Combine(folderPath, SAVE_FILE_NAME);

        // Verificar si la carpeta existe, si no, crearla
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (File.Exists(filePath))
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                // Deserializar de manera as�ncrona
                BinaryFormatter formatter = new BinaryFormatter();
                return await Task.FromResult(formatter.Deserialize(stream) as List<ItemLocal>);
            }
        }
        else
        {
            Debug.Log("Archivos locales no encontrados");
            return new List<ItemLocal>();
        }
    }

    public async Task SaveLocalItemsAsync(List<ItemLocal> listItemsLocal)
    {
        if (!IsUserFolderNameUid()) return;

        string folderPath = Path.Combine(Application.persistentDataPath, folderNameUser);
        string filePath = Path.Combine(folderPath, SAVE_FILE_NAME);

        // Verificar si la carpeta existe, si no, crearla
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        await Task.Run(() =>
        {
            // Serializar y guardar de manera as�ncrona
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, listItemsLocal);
            }
        });
    }

    private bool IsUserFolderNameUid()
    {
        if (folderNameUser == null)
        {
            Debug.LogWarning("No hay un userUid para nombre de la carpeta de usuario!!!");
            return false;
        }
        return true;
    }
}
