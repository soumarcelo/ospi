namespace Engine.Domain.Enums;

public enum TransactionExternalReferenceType
{
    EndToEnd, // E2E para PIX
    NSU,      // Número Sequencial Único
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed,
    Cancelled,
    Reversed,
}

public enum TransactionFailureReasonCode
{
    PermissionDenied, // Permissão negada
    InvalidUser, // Usuário inválido
    InvalidTransaction, // Transação inválida
    InsufficientFunds, // Fundos insuficientes
    CounterpartyError, // Erro na contraparte
    AccountError, // Erro na conta
    InvalidAccount,    // Conta inválida
    NetworkError,      // Erro de rede
    Timeout,           // Tempo esgotado
    FraudDetected,     // Fraude detectada
    UserCancelled,    // Usuário cancelou
    SystemError,       // Erro do sistema
    InvalidPixKey,   // Chave PIX inválida
    TransactionLimitExceeded, // Limite de transação excedido
    DuplicateTransaction, // Transação duplicada
    InvalidAmount,     // Valor inválido
    AccountBlocked,    // Conta bloqueada
    PaymentGatewayFailure, // Falha no gateway de pagamento
    RegulatoryIssue,   // Questão regulatória
    Other,             // Outro motivo
}

public enum TransactionDirection
{
    Incoming,
    Outgoing,
}

public enum TransactionType
{
    Transfer,
    Payment,
    Refund,
}

public enum TransactionMethodType
{
    Pix,
    Ted,
    Boleto,
    CreditCard,
}