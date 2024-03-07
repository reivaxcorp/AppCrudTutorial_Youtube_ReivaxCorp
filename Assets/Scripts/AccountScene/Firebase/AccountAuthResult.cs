/*********************************************************************************
 * Nombre del Archivo:     AccountAuthResult.cs
 * Descripci�n:            Clase que nos ayudar� en manejar los resultados de auntentificaci�n de el 
 *                         usuario, inicio sesi�n, creaci�n de cuentas, login. Que se enviar� a la 
                           Ui de la aplicaci�n
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


public enum AuthType
{
    LOGOUT, LOGIN_SUCCESS, LOGIN_FAILURE, LOGIN_CANCEL,
    CREATE_ACCOUNT_SUCCESS, CREATE_ACCOUNT_FAILURE, CREATE_ACCOUNT_CANCEL,
    SEND_MAIL_VERIFICATION_SUCCESS, SEND_MAIL_VERIFICATION_CANCEL, SEND_MAIL_VERIFICATION_FAILURE
}

/// <summary>
/// Get result when we do auth actions.
/// </summary>
public class AccountAuthResult
{

    public string Message { get => _message; private set => _message = value; }
    public AuthType AuthType { get => _authType; private set => _authType = value; }

    private string _message;
    private AuthType _authType;

    public AccountAuthResult(AuthType authType, string message)
    {
        Message = message;
        AuthType = authType;
    }
}