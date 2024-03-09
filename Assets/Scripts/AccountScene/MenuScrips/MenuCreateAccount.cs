/*********************************************************************************
 * Nombre del Archivo:     MenuCreateAccount.cs
 * Descripci�n:            Como su nombre lo indica, aqu� es donde crearemos nuestra cuenta de usuario.
 *                         Una vez creada la cuenta, el usuario debera confirmar su direcci�n de email, para 
 *                         poder utilizar la aplicaci�n. 
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

using UnityEngine;
using TMPro;

public class MenuCreateAccount : MenuAuth
{
    [SerializeField] private TMP_InputField inputMail;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TMP_InputField inputRePassword;

    public void CreateAccountWithMailAndPassword()
    {
        if (IsInputsSetted())
        {
            if (validateInputs.IsValidEmail(inputMail.text, resultMsj))
            {
                if (validateInputs.IsFormatPasswordCorrect(inputPassword, inputRePassword, resultMsj))
                {
                    ShowScreenLoading(true);

                    string mail = inputMail.text;
                    string password = inputPassword.text;

                    firebaseAuthManager.OnAccountAuthResult += SetResult;
                    firebaseAuthManager.CreateAccountWithMailAndPassword(mail, password);
                    ClearInputs();
                }
            }
        }
    }

    private void ClearInputs()
    {
       inputMail.text = "";
       inputPassword.text = "";
       inputRePassword.text = "";
    }


    private bool IsInputsSetted()
    {
        if (inputMail == null)
        {
            Debug.LogWarning("Please put inputMail on inspector");
            return false;
        }
        if (inputPassword == null)
        {
            Debug.LogWarning("Please put inputPassword on inspector");
            return false;
        }
        if (inputRePassword == null)
        {
            Debug.LogWarning("Please put inputRePassword on inspector");
            return false;
        }
        return true;
    }
}

