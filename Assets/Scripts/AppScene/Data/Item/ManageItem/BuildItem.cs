/*********************************************************************************
 * Nombre del Archivo:     BuildItem.cs
 * Descripci�n:            Clase encargada de asignar la textura a nuestro �tem leyendo el 
 *                         archivo guardado localmente en nuestro dispositivo � descargando y 
 *                         salvandolo y asignarlo en el �tem.  
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


public class BuildItem: MonoBehaviour
{
 
    public async Task AsignMaterialAsync(string imageName, GameObject gameObject)
    {
       
        Texture2D texture2D = GetSavedTexture(imageName);

        // si no esta la imagen, es que se actualizo anteriormente en otro dispositivo
        if(texture2D == null)
        {
            ManageStorageRemote createMaterial = new ManageStorageRemote(imageName);
            texture2D = await createMaterial.DownloadImage();
            FileManager fileManager = new FileManager(FirebaseSDK.GetInstance().auth.CurrentUser.UserId);
            fileManager.SaveFileInternalExtorage(texture2D, imageName);
        }

        try
        {
            Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            newMaterial.mainTexture = texture2D;
            newMaterial.SetTexture("_MainTex", texture2D);

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            Material[] currentMaterials = meshRenderer.materials;
            currentMaterials[0] = newMaterial;
            meshRenderer.materials = currentMaterials;
        } 
        catch (Exception ex)
        {
            Debug.LogWarning("Error al aplicar la textura " + ex.Message);
        }

    }
 
    // Obtiene la textura desde nuestro dispositivo interno
    private Texture2D GetSavedTexture(string imageName)
    {
        FileManager fileManager = new FileManager(FirebaseSDK.GetInstance().auth.CurrentUser.UserId);
        return fileManager.LoadFileAsTexture2D(imageName);
    }
}
