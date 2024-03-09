/*********************************************************************************
 * Nombre del Archivo:     BuildItem.cs
 * Descripción:            Clase encargada de asignar la textura a nuestro ítem leyendo el 
 *                         archivo guardado localmente en nuestro dispositivo ó descargando y 
 *                         salvandolo y asignarlo en el ítem.  
 *                         
 * Autor:                  Javier
 * Organización:           ReivaxCorp.
 *
  * Derechos de Autor © [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentación asociados (el “Software”), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluirán en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, específicamente en las carpetas “Assets/Scripts”.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D están sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, así como a las políticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA “TAL CUAL”, SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROPÓSITO PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SERÁN RESPONSABLES DE CUALQUIER RECLAMACIÓN, DAÑO U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCIÓN DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXIÓN CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
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
