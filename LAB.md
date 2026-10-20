# Products API Lab Log

## Part 0 — Baseline
At the beginning, the API had no validation or error handling.  
- **POST invalid product** (empty name, negative price):
  - Output: Accepted with 200/201, even though the data was invalid.
- **GET /api/Products/9999** (missing id):
  - Output: Either a raw 500 error or an empty 200 response.

This baseline confirmed the weaknesses: invalid input was allowed, and missing resources were not handled properly.

---

## Part 1 — Validation
I added Data Annotations to the request models (`CreateProductRequest`, `UpdateProductRequest`) and confirmed `[ApiController]` was active.  
- POST invalid product** (empty name, negative price):
  - Output:
  json
  {
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
      "Name": [
        "The Name field is required.",
        "The field Name must be a string with a minimum length of 2 and a maximum length of 100."
      ],
      "Price": [
        "The field Price must be between 0.01 and 100000."
      ]
    }
  }
 

Now invalid input is rejected with **400 Bad Request** and detailed error messages.

---

## Part 2 — Custom Rule and Domain Exceptions
I added a custom rule: **SKU must be unique**. I also created domain exceptions (`NotFoundException`, `ConflictException`).  
- POST duplicate SKU**:
  - Output (before global handler):
  json
  {
    "error": "ConflictException: SKU ABC-1234 already exists."
  }
  
- GET /api/Products/9999**:
  - Output (before global handler):
  json
  {
    "error": "NotFoundException: Product 9999 not found."
  }
  

At this stage, rules were enforced but exceptions were still raw.


## Part 3 — Global Exception Handling
I implemented `GlobalExceptionHandler` to map exceptions to proper status codes.  
- GET /api/Products/9999**:
  - Output:
  json
  {
    "title": "Resource not found",
    "status": 404,
    "detail": "Product 9999 not found."
  }
  
- **POST duplicate SKU**:
  - Output:
  json
  {
    "title": "Conflict",
    "status": 409,
    "detail": "SKU ABC-1234 already exists."
  }
  

Errors are now clean, standardized, and safe for clients.

---

## Part 4 — Standardized Responses
I ensured all endpoints return correct success codes and predictable formats.  
- **POST new product**:
  - Output: **201 Created** with Location header.
- PUT existing product**:
  - Output:
  json
  {
    "name": "Gaming Monitor",
    "price": 350,
    "sku": "MON-4321"
  }
  
- DELETE existing product**:
  - Output: **204 No Content**.
- **GET /api/Products?page=1&pageSize=2**:
  - Output:
  json
  {
    "data": [
      { "name": "Keyboard", "price": 100, "sku": "ABC-1234" },
      { "name": "Mouse", "price": 50, "sku": "XYZ-5678" }
    ],
    "meta": {
      "page": 1,
      "pageSize": 2,
      "totalItems": 5,
      "totalPages": 3
    }
  }
  

Now every endpoint has predictable success codes and standardized error responses.

---

## Reflection
Step by step, the API evolved from a fragile in‑memory CRUD into a production‑grade service. Validation stopped bad input, domain exceptions expressed business rules, and global exception handling centralized error mapping. Finally, standardized success codes and paging made the API predictable and professional. Together, these improvements show how validation, exceptions, and response standards work in harmony to produce a reliable Web API.
   