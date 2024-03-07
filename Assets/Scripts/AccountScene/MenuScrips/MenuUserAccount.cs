/*********************************************************************************
 * Nombre del Archivo:     MenuUserAccount.cs
 * Descripci�n:            El panel de usuario, el encargado de llevarnos a la pantalla para poder crear nuestros 
 *                         �tems, siempre y cuando el usuario ya haya confirmado su direcci�n de email, de lo contrario
 *                         no podr� ver sus �tems. 
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
                    // debemos salir de la sesi�n de usuario, ya que de otra manera
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
            // Procedemos a enviar un email de verificaci�n de mail.
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
