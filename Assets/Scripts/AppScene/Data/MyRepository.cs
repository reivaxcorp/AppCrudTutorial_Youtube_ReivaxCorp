/*********************************************************************************
 * Nombre del Archivo:     MyRepository.cs
 * Descripci�n:            Patr�n repositorio, nos ayudar� a comunicarnos con la base de datos local
 *                         y remota, con sus interfaces correspondientes, para aislar la capa de datos de la
 *                         aplicaci�n.  
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor (c) [2024] ReivaxCorp
 * 
 * Permiso es otorgado, sin cargo, para que cualquier persona obtenga una copia
 * de este software y de los archivos de documentaci�n asociados (el "Software"),
 * para tratar en el Software sin restricci�n, incluyendo sin limitaci�n los
 * derechos para usar, copiar, modificar, fusionar, publicar, distribuir,
 * sublicenciar, y/o vender copias del Software, y para permitir a las personas a
 * quienes pertenezca el Software, sujeto a las siguientes condiciones:
 *
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en
 * todas las copias o partes sustanciales del Software.
 *
 * EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANT�A DE NING�N TIPO, EXPRESA O
 * IMPL�CITA, INCLUYENDO PERO NO LIMITADO A LAS GARANT�AS DE COMERCIABILIDAD,
 * IDONEIDAD PARA UN PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS
 * AUTORES O TITULARES DE DERECHOS DE AUTOR SER�N RESPONSABLES DE CUALQUIER
 * RECLAMACI�N, DA�O O OTRA RESPONSABILIDAD, YA SEA EN UNA ACCI�N DE CONTRATO, AGRAVIO
 * O DE OTRO MODO, DERIVADAS DE, FUERA DE O EN CONEXI�N CON EL SOFTWARE O EL USO U OTROS
 * TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MyRepository : IRepositoryLocal, IRepositoryRemote, IRepositoryStorage
{
    private IRepositoryLocal localDb;
    private IRepositoryRemote remoteDb;

    public MyRepository(IRepositoryLocal localDb, IRepositoryRemote remoteDb)
    {
        this.localDb = localDb;
        this.remoteDb = remoteDb;
    }

    public async Task<bool> DeleteItemRemoteById(string id, IResult iResultUi)
    {
        return await remoteDb.DeleteItemRemoteById(id, iResultUi);
    }
   
    public async Task<List<ItemLocal>> GetLocalItemsAsync()
    {
        return await localDb.GetLocalItemsAsync();
    }

    public async Task SaveLocalItemsAsync(List<ItemLocal> listItemsLocal)
    {
        await localDb.SaveLocalItemsAsync(listItemsLocal);
    }

    public async Task<bool> SaveItemRemote(ItemRemote itemRemote, IResult resultUi)
    {
        return await remoteDb.SaveItemRemote(itemRemote, resultUi);
    }

    public async Task<bool> UpdateItemRemote(ItemRemote itemRemote, IResult resultUi)
    {
        return await remoteDb.UpdateItemRemote(itemRemote, resultUi);
    }

    public async Task<ItemLocal> GetLocalItemById(string id)
    {
        return await localDb.GetLocalItemById(id);
    }

    public async Task<bool> UploadFileFirebaseStorage(string generateImageName, string folderNameUser, byte[] fileBytes)
    {
        ManageStorageRemote manageStorageRemote =
                     new ManageStorageRemote(generateImageName, folderNameUser, fileBytes);
        return await manageStorageRemote.UploadFileFirebaseStorage();
    }

    public async Task<Texture2D> DowloadImageStorage(string imageName)
    {
        ManageStorageRemote createMaterial = new ManageStorageRemote(imageName);
        return await createMaterial.DownloadImage();
    }

    public async Task<bool> DeleteImageStorage(string imageName)
    {
        ManageStorageRemote manageMaterialRemote =
                   new ManageStorageRemote(imageName);
        return await manageMaterialRemote.DeleteImageRemote();
    }

    public RemoteDb GetRemoteDb()
    {
        return remoteDb as RemoteDb;
    }

    public LocalDb GetLocalDb()
    {
        return localDb as LocalDb;
    }
}