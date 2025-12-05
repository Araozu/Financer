namespace Financer.Domain.Utils;

public record Success;

public record BadRequest(string Reason);

public record BadRequest<T>(T data)
    where T : notnull;

public record NotFound(string Reason);

public record NotFound<T>(T data)
    where T : notnull;

public record Unauthorized(string Reason);

public record Unauthorized<T>(T data)
    where T : notnull;

public record InternalError(string Reason);

public record InternalError<T>(T data)
    where T : notnull;