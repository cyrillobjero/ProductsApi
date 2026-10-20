# Products API

## Status Code Table

| Endpoint                  | Success Response                  | Error Responses                          |
|---------------------------|-----------------------------------|------------------------------------------|
| GET /api/Products         | 200 OK (paged {data, meta} envelope) | 500 Internal Server Error (unexpected)   |
| GET /api/Products/{id}    | 200 OK (single product)            | 404 Not Found (ProblemDetails)           |
| POST /api/Products        | 201 Created (+ Location header)    | 400 Bad Request (validation errors, ProblemDetails)<br>409 Conflict (duplicate SKU, ProblemDetails) |
| PUT /api/Products/{id}    | 200 OK (updated product)           | 400 Bad Request (validation errors)<br>404 Not Found (ProblemDetails) |
| DELETE /api/Products/{id} | 204 No Content                     | 404 Not Found (ProblemDetails)           |
