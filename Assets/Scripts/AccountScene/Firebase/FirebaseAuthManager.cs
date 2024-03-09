/*********************************************************************************
 * Nombre del Archivo:     FirebaseAuthManager.cs
 * Descripci�n:            LLamadas al sdk de firebase para la craci�n de cuentas e inicio de sesi�n de 
 *                         usuario, nos ayudamos por medio de eventos, al suscribirnos en las clases 
 *                         correspondientes. 
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

using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// Manejar las acciones de auntentificaci�n.
/// </summary>
public class FirebaseAuthManager
{

    public delegate void AuthCallback(AccountAuthResult result);
    public event AuthCallback OnAccountAuthResult;
    private ExceptionManager exceptionManager;

    public FirebaseAuthManager()
    {
        this.exceptionManager = new ExceptionManager();
    }

    public void CreateAccountWithMailAndPassword(string email, string password)
    {
        if (FirebaseSDK.GetInstance().isFirebaseReady)
        {
            FirebaseSDK.GetInstance().auth.CreateUserWithEmailAndPasswordAsync(email, password)
                .ContinueWithOnMainThread(task =>
            {
                AccountAuthResult authResult;

                if (task.IsCanceled)
                {
                    Debug.LogError("Was canceled.");
                    authResult = new AccountAuthResult(AuthType.CREATE_ACCOUNT_CANCEL, "�Creaci�n de cuenta cancelada!");
                    OnAccountAuthResult?.Invoke(authResult);
                    return;
                }
                if (task.IsFaulted)
                {
                    authResult = new AccountAuthResult(AuthType.CREATE_ACCOUNT_FAILURE, exceptionManager.ManageExceptionForm(task));
                    OnAccountAuthResult?.Invoke(authResult);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                authResult = new AccountAuthResult(AuthType.CREATE_ACCOUNT_SUCCESS, "Cuenta creada");
                OnAccountAuthResult?.Invoke(authResult); // we need TaskScheduler.FromCurrentSync.... to set text
            });

        }
        else
        {
            Debug.LogWarning("Firebase isn't running!");
        }
    }

    public void LoginWithExistingAccount(string email, string password)
    {

        if (FirebaseSDK.GetInstance().isFirebaseReady)
        {
            FirebaseSDK.GetInstance()
                .auth
                .SignInWithEmailAndPasswordAsync(
                email,
                password)
                .ContinueWithOnMainThread(task =>
                {
                    AccountAuthResult authResult;

                    if (task.IsCanceled)
                    {
                        Debug.LogError("Was canceled.");
                        authResult = new AccountAuthResult(AuthType.LOGIN_CANCEL, "�Login cancelado!");
                        OnAccountAuthResult?.Invoke(authResult);
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        authResult = new AccountAuthResult(AuthType.LOGIN_FAILURE, exceptionManager.ManageExceptionForm(task));
                        OnAccountAuthResult?.Invoke(authResult);
                        return;
                    }

                    AuthResult result = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                    authResult = new AccountAuthResult(AuthType.LOGIN_SUCCESS,"Logeado como: \n" + result.User.Email);
                    OnAccountAuthResult?.Invoke(authResult);
                });
        }
    }

    public void SendEmailUserVerification()
    {
        if (FirebaseSDK.GetInstance().isFirebaseReady)
        {
            FirebaseSDK.GetInstance().auth.CurrentUser.SendEmailVerificationAsync()
                .ContinueWithOnMainThread(task =>
                {
                    AccountAuthResult authResult;

                    if (task.IsCanceled)
                    {
                        Debug.LogError("SendEmailVerificationAsync was canceled.");
                        authResult = new AccountAuthResult(AuthType.SEND_MAIL_VERIFICATION_CANCEL, "Email de verificaci�n cancelado");
                        OnAccountAuthResult?.Invoke(authResult);
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                        authResult = new AccountAuthResult(AuthType.SEND_MAIL_VERIFICATION_FAILURE, "Error al enviar el email de verificaci�n");
                        OnAccountAuthResult?.Invoke(authResult);
                        return;
                    }

                    authResult = new AccountAuthResult(AuthType.SEND_MAIL_VERIFICATION_SUCCESS, "Se acaba de enviar el email de verificaci�n\nVerifica tu mail e inicia sesi�n");
                    OnAccountAuthResult?.Invoke(authResult);
                    Debug.Log("Email sent successfully.");
                });
        }
    }

    public void LogOut()
    {
        FirebaseSDK.GetInstance().LogOut();
        AccountAuthResult result = new AccountAuthResult(AuthType.LOGOUT, "Deslogeado");
        OnAccountAuthResult?.Invoke(result);
    }

}
