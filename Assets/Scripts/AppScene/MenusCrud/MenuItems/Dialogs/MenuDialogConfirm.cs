/*********************************************************************************
* Nombre del Archivo:     MenuDialogConfirm.cs
* Descripci�n:            Cuadro de dialogo que nos mostrara el resultado de las acciones CRUD.  
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

using TMPro;
using UnityEngine;

public class MenuDialogConfirm : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTitle;
    [SerializeField] TextMeshProUGUI textBody;
    [SerializeField] MenuManagerApp menuManager;

    private void Awake()
    {
        CheckReferences();
    }

    public void ShowDialog(string title, string message)
    {
        SetTitle(title);
        SetBodyText(message);
        ShowDialog();
    }

    // el usuario cerro con la "X" el dialogo de confirmacion
    public void OnClosed()
    {
        HideDialog();
    }

    public void OnAccept()
    {
        HideDialog();
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    public void SetTitle(string title)
    {
        textTitle.text = title;
    }

    public void SetBodyText(string bodyText)
    {
        textBody.text = bodyText;
    }

    private void HideDialog()
    {
        menuManager.HideUiButtons(false);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ClearDialog();
    }

    private void ClearDialog()
    {
        textTitle.text = "";
        textBody.text = "";
    }

    private void CheckReferences()
    {
        if (textTitle == null) Debug.LogWarning("Pon la referencia Title en el inspector");
        if (textBody == null) Debug.LogWarning("Pon la referencia Msj en el inspector");
        if (menuManager == null) Debug.LogWarning("Pon la reference de MenuManager en el inspector");
    }

}
