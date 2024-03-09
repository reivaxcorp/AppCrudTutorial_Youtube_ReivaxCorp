/*********************************************************************************
 * Nombre del Archivo:     MenuAddItem.cs
 * Descripci�n:            Clase responsable de a�adir nuevos �tems.  
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
using UnityEngine;

public class MenuAddItem : MenuCrud, IResult
{
    private string generateImageName;

    public void SetResultCrudUi(string title, string msj)
    {
        StartAnimationTextMenu(false, "");
        uiApp.MenuSetActive(false);
        OpenDialog(title, msj);
    }

    /// <summary>
    /// Cuando colocamos subir �tem, lo primero que hacemos es subir la imag�n, luego escribimos
    /// los datos en realtime database, con WriteDocument, si la subida se realiz� correctamente.
    /// </summary>
    public async void CreateDocumentRemote()
    {
        if (AppConfig.IsItemAvariableToPut())
        {
            if (IsDataSetted())
            {
                StartAnimationTextMenu(true, "Creando");
                // Obtenemos los bytes de la imag�n temporal seleccionada
                byte[] fileBytes = fileManager.GetBytesImageSelected();
                // Generar nombre de imag�n aleatorea
                generateImageName = Guid.NewGuid().ToString();
                // subir nueva imag�n
                bool uploadResult = await MyApplication.repository.UploadFileFirebaseStorage(generateImageName, fileManager.folderNameUser, fileBytes);

                if (uploadResult)
                {
                    fileManager.ChangeNameImageCopySelected(generateImageName);
                    WriteDocumentRemote(generateImageName);
                }
                else
                {
                    SetResultCrudUi("Error", "Error al subir el documento");
                }
            }
        }
        else
        {
            SetMsjInfoUI("�tems maximos alcanzados, edita � borra uno");
        }
    }

    /// <summary>
    /// Una vez subida la imag�n, procedemos a escribir el documento.
    /// </summary>
    /// <param name="imageName"></param>
    private async void WriteDocumentRemote(string imageName)
    {
        if (MyApplication.repository != null)
        {
            ItemRemote itemRemote = new ItemRemote(name: inputFieldName.text, imageName: imageName);
            await MyApplication.repository.SaveItemRemote(itemRemote, resultUi: this);
        }
        else
        {
            Debug.LogWarning("El repositorio es Null");
        }
    }

}
