/*********************************************************************************
 * Nombre del Archivo:     MenuUpdateItem.cs
 * Descripci�n:            Encargado de actualizar �tems desde su menu, asi como tambi�n
 *                         borrar items, y comunicarse con las clases correspondientes de manejo
 *                         para el manejo de archivos.
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

using System;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Comprobamos la actulizaci�n de los items cuando hacemos touch en alguno
/// Si el usuario eligio una nueva imagen se actualizar� en cambio no lo har�
/// </summary>
public class MenuUpdateItem : MenuCrud, IResult, IResultDialogDelete
{
    [SerializeField] DialogDeleteConfirm dialogDeleteConfirm;
    private ItemLocal currentItemSelected;
    private string oldImageName;
    private string generateImageName;

    public void SetResultCrudUi(string title, string msj)
    {
        StartAnimationTextMenu(false, "");
        uiApp.MenuSetActive(false);
        OpenDialog(title, msj);
    }

    /// <summary>
    /// Cuando abrimos el menu, seteamos los valores.
    /// </summary>
    /// <param name="itemId"></param>
    public async void InitMenu(string itemId)
    {
        if (MyApplication.repository != null)
        {
            try
            {
                ItemLocal itemLocal = await MyApplication.repository.GetLocalItemById(itemId);
                this.currentItemSelected = itemLocal;
                FileManager fileManager = new FileManager(FirebaseSDK.GetInstance().auth.CurrentUser.UserId);
                Texture2D texture2D = fileManager.LoadFileAsTexture2D(itemLocal.ImageName); 
                SetImageNameInInput(itemLocal.Name);
                SetImagePreview(texture2D);
                this.oldImageName = itemLocal.ImageName;
            }
            catch (Exception exception)
            {
                Debug.LogError("Error en la textura o el �tem no existe! " + exception);
            }
        }
        else
        {
            Debug.LogWarning("El repositorio es null");
        }
    }
 
    /// <summary>
    /// Si se cambia la imag�n, debemos subirla, y luego si se sube correctamente,
    /// debemos borrar la imag�n anterior de firebase storage, por medio de la interface
    /// IResultFile -> FileUploaded m�todo
    /// </summary>
    public async void UpdateItem()
    {
        if (IsDataSetted())
        {
            if(IsSomeDatachanged())
            {
                try
                {
                    StartAnimationTextMenu(false, "Actualizando");

                    if (isImageChanged)
                    {
                         await UpdateImageRemote();
                    }
                    else
                    {
                        // solo se actualizaron los campos y no la imag�n
                        UpdateDocumentRemote();
                    }
                }
                catch (Exception excepcion)
                {
                    SetResultCrudUi("Error", "Error - " + excepcion.Message);
                }
            }
            else
            {
                SetResultCrudUi("Todo igual", "Nada ha cambiado....");
            }
        }
    }

    public void DeleteItem()
    {
        if(dialogDeleteConfirm != null)
        {
            dialogDeleteConfirm.ShowDialog("Borrar �tem" , "�Deseas eliminar el �tem?", this);
        } else
        {
            Debug.LogWarning("DialogDeleteConfirm es null, colocalo en el inspector");
        }
    }
     
    private async Task<bool> UpdateImageRemote()
    {
        // Obtenemos los bytes de la imag�n temporal seleccionada
        byte[] fileBytes = fileManager.GetBytesImageSelected();
        // Generar nombre de imag�n aleatorea
        generateImageName = Guid.NewGuid().ToString();
        ManageStorageRemote manageStorageRemote =
            new ManageStorageRemote(generateImageName, fileManager.folderNameUser, fileBytes);
        // subir nueva imag�n
        bool resultUpload = await manageStorageRemote.UploadFileFirebaseStorage();
        fileManager.ChangeNameImageCopySelected(generateImageName);
        // borrar imag�n anterior
        await DeleteImageRemote();
        // actualizar documento
        UpdateDocumentRemote();
        return resultUpload;
    }

    private async void UpdateDocumentRemote()
    {
        // �tem a actualizar
        ItemRemote itemRemote = new ItemRemote(
            id: currentItemSelected.Id,
            name: inputFieldName.text,
            imageName: isImageChanged ? generateImageName : oldImageName,
            creationDate: currentItemSelected.CreationDate);
        
        // actualizamos el documente de firebase realtimadatabase
        bool updateResult = await MyApplication.repository.UpdateItemRemote(itemRemote, resultUi: this);
        if (updateResult) { Invoke("ShowInterstitialAd", AppConfig.timeInterstitialAd); }
    }

    /// <summary>
    /// borramos imag�n anterior remoto, cuando ya hemos colocado otra imag�n nueva
    /// de firebase storage, o borramos la imag�n cuando borramos el �tem
    /// </summary>
    private async Task<bool> DeleteImageRemote()
    {
        // imag�n de firebase storage
        ManageStorageRemote manageMaterialRemote =
                     new ManageStorageRemote(currentItemSelected.ImageName);
        await manageMaterialRemote.DeleteImageRemote();
        return true;
    }
 
    /// <summary>
    /// Verificamos si el usuario cambio algo en la Ui
    /// </summary>
    /// <returns></returns>
    private bool IsSomeDatachanged()
    {
        return isImageChanged || !inputFieldName.text.Equals(currentItemSelected.Name);
    }

    public async void ConfirmDialogDelete(bool isDeleteConfirm)
    {
        if(isDeleteConfirm)
        {
            await DeleteImageRemote();
            bool deleteResult = await MyApplication.repository.DeleteItemRemoteById(currentItemSelected.Id, iResultUi: this);
            if (deleteResult) { Invoke("ShowInterstitialAd", AppConfig.timeInterstitialAd); }
        }
    }

}
