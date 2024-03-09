/*********************************************************************************
 * Nombre del Archivo:     MenuManagerApp.cs 
 * Descripción:            Clase que se encargará de la interacción del usuario con los botónes
 *                         de la interfaz de usuario.
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerApp : MonoBehaviour
{

    [Header("Menu CRUD")]
    [SerializeField] GameObject menuAddItem;
    [SerializeField] GameObject menuUpdateItem;

    [Header("Botones Main UI")]
    [SerializeField] GameObject addItemBtn;
    [SerializeField] GameObject menuCompanyBtn;
    [SerializeField] GameObject backBtn;

    [Header("APP info")]
    [SerializeField] GameObject menuCompany;
    [SerializeField] GameObject myItemsOrdened;
    [SerializeField] GameObject tutorialInfo;

    private MenuCrud menu;

    public void ShowMenuAddItem()
    {
        if (!menuAddItem.activeSelf && !menuUpdateItem.activeSelf)
        {
            menuAddItem.transform.parent.gameObject.SetActive(true);
            menuAddItem.SetActive(true);
            HideUiButtons(true);
        }
    }

    public void ShowMenuUpdateItem(string idItem)
    {
        if (!menuUpdateItem.activeSelf && !menuAddItem.activeSelf)
        {
            menuUpdateItem.transform.parent.gameObject.SetActive(true);
            menuUpdateItem.SetActive(true);
            HideUiButtons(true);
            menuUpdateItem.GetComponent<MenuUpdateItem>().InitMenu(idItem);
        }
    }

    public void ShowMenuCompany()
    {
        if(!menuCompany.activeSelf)
        {
            menuCompany.SetActive(true);
            HideUiButtons(true);
        }
    }

    public void HideMenuCompany()
    {
        if (menuCompany.activeSelf)
        {
            menuCompany.SetActive(false);
            HideUiButtons(false);
        }
    }


    public void SetCurrentMenu(MenuCrud menu)
    {
        this.menu = menu;
    }

    public void ShowMenu()
    {
        HideUiButtons(true);
        MenuSetActive(true);
    }

    public void HideMenu()
    {
        MenuSetActive(false);
        HideUiButtons(false);
    }

    public void HideUiButtons(bool isActive)
    {
        addItemBtn.SetActive(!isActive);
        backBtn.SetActive(!isActive);
        menuCompanyBtn.SetActive(!isActive);
        tutorialInfo.SetActive(!isActive);
    }

    public void MenuSetActive(bool isActive)
    {
        if (menu != null)
        {
            menu.gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning("Esta llamando un menú que es null");
        }
    }

    public void GoBack()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }


    private void Awake()
    {
        CheckReferences();
    }

    private void CheckReferences()
    {
        if (addItemBtn == null) { Debug.LogWarning("Por favor, por la referencia AddItemBtn (child MenuApp gameObject) en el  inspector"); }
        if (menuAddItem == null) { Debug.LogWarning("Por favor, por el gameobject MenuAddItem en el inspector"); }
        if(menuCompany == null) { Debug.LogWarning("Por favor, por el gameobject MenuCompany en el inspector"); }
        if (backBtn == null) { Debug.LogWarning("Por favor, por el gameobject BackBtn en el inspector"); }
        if(menuCompanyBtn == null) { Debug.LogWarning("Por favor, por el gameobject MenuCompanyBtn en el inspector"); }
        if(tutorialInfo == null) { Debug.LogWarning("Por favor, por el gameObject TutorialInfo en el inspector"); }
    }
}
