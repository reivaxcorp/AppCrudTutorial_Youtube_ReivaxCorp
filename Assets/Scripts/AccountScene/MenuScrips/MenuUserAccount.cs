/*********************************************************************************
 * Nombre del Archivo:     MenuUserAccount.cs
 * Descripción:            El panel de usuario, el encargado de llevarnos a la pantalla para poder crear nuestros 
 *                         ítems, siempre y cuando el usuario ya haya confirmado su dirección de email, de lo contrario
 *                         no podrá ver sus ítems. 
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

using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUserAccount : MenuAuth
{
    [SerializeField] private GameObject loginBtn;
    [SerializeField] private GameObject misItemsBtn;
    [SerializeField] private GameObject logOutBtn;

    private bool getUserStatus;

    void Update()
    {
        ReadUserStatus();
    }

    public override void SetResult(AccountAuthResult result)
    {
        if (resultMsj != null)
        {

            resultMsj.SetText(result.Message);

            switch (result.AuthType)
            {
                case AuthType.LOGOUT:
                    resultMsj.color = Color.gray;
                    HideButtonsSessionOn();
                    break;
                case AuthType.LOGIN_SUCCESS:
                    resultMsj.color = Color.green;
                    ShowButtonsSessionOn();
                    break;
                case AuthType.LOGIN_FAILURE:
                    resultMsj.color = Color.red;
                    HideButtonsSessionOn();
                    break;
                case AuthType.LOGIN_CANCEL:
                    resultMsj.color = Color.gray;
                    HideButtonsSessionOn();
                    break;
                case AuthType.SEND_MAIL_VERIFICATION_SUCCESS:
                    resultMsj.color = Color.green;
                    // debemos salir de la sesión de usuario, ya que de otra manera
                    // la propiedad IsEmailVerified, nos devolvera false
                    FirebaseSDK.GetInstance().LogOut();
                    break;
                case AuthType.SEND_MAIL_VERIFICATION_CANCEL:
                    resultMsj.color = Color.gray;
                    break;
                case AuthType.SEND_MAIL_VERIFICATION_FAILURE:
                    resultMsj.color = Color.red;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.LogWarning("msj result menu es null");
        }
    }

    public void LogOut()
    {
        firebaseAuthManager.OnAccountAuthResult += SetResult;
        firebaseAuthManager.LogOut();
    }

    public void LoadSceneMyItems()
    {
        ShowScreenLoading(true);
        SceneManager.LoadScene("AppScene");
    }

    private void ReadUserStatus()
    {
        if (!getUserStatus)
        {
            if (FirebaseSDK.GetInstance().isFirebaseReady &&
                FirebaseSDK.GetInstance().auth.CurrentUser != null)
            {
                getUserStatus = true;
                VerifyMail();
            }
        }
    }

    private void VerifyMail()
    {
        FirebaseUser user =
             FirebaseSDK.GetInstance().auth.CurrentUser;

        if (user.IsEmailVerified)
        {
            AccountAuthResult result = new AccountAuthResult(AuthType.LOGIN_SUCCESS, "Logeado con email: \n" + FirebaseSDK.GetInstance().auth.CurrentUser.Email);
            SetResult(result);
        }
        else
        {
            // Procedemos a enviar un email de verificación de mail.
            HideButtonsSessionOn();
            firebaseAuthManager.OnAccountAuthResult += SetResult;
            firebaseAuthManager.SendEmailUserVerification();
        }
    }

    private void HideButtonsSessionOn()
    {
        if (misItemsBtn != null && logOutBtn != null)
        {
            misItemsBtn.SetActive(false);
            logOutBtn.SetActive(false);
            ShowLoginButton(true);
            ClearMsjResult();
        }
        else
        {
            Debug.LogWarning("Please put btn on inspector EnterBtn and LogOutbtn");
        }
    }

    private void ShowButtonsSessionOn()
    {
        if (misItemsBtn != null && logOutBtn != null)
        {
            misItemsBtn.SetActive(true);
            logOutBtn.SetActive(true);
            ShowLoginButton(false);
        }
        else
        {
            Debug.LogWarning("Please put btn on inspector EnterBtn and LogOutbtn");
        }
    }

    private void ShowLoginButton(bool isVisible)
    {
        if (loginBtn != null)
        {
            loginBtn.SetActive(isVisible);
        }
        else
        {
            Debug.LogWarning("Please put Loginbtn on inspector");
        }
    }

    // when we disable, reset variable for next time reload menu
    private void OnDisable()
    {
        ClearMsjResult();
        getUserStatus = false;
    }

}
