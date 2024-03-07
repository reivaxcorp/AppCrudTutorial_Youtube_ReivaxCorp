/*********************************************************************************
 * Nombre del Archivo:     ExceptionManager.cs
 * Descripci�n:            Los errores que nos pueda lanzar firebase auth, los errores, ser�n 
 *                         notificados en la Ui. 
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

public class ExceptionManager
{
    /// <summary>
    /// Manage firebase Exception
    /// </summary>
    /// <param name="task"></param>
    public string ManageExceptionForm(Task task)
    {
        AggregateException exception = task.Exception.Flatten();

        foreach (Exception innerException in exception.InnerExceptions)
        {
            Firebase.FirebaseException firebaseException = innerException as Firebase.FirebaseException;

            if (firebaseException != null)
            {
                // Check if the email address is already in use
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.EmailAlreadyInUse)
                {
                    return "El correo electronico ya se esta usando en otra cuenta";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.WrongPassword)
                {
                    return "Contrase�a incorrecta";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserNotFound)
                {
                    return "Cuenta no encontrada";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserMismatch)
                {
                    return "Usuario no coincidente";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserDisabled)
                {
                    return "Usuario deshabilitado!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.InvalidCredential)
                {
                    return "Credenciales invalidas!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.InvalidAppCredential)
                {
                    return "Credenciales de aplicaci�n invalidas!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.RejectedCredential)
                {
                    return "Credencial rechazada!";
                }
            }
        }

        Debug.LogError("Error. int�ntelo nuevamente : " + task.Exception);
        return "Error. int�ntelo nuevamente";
    }
}