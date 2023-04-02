Requirement: Create an Asp.Net Core Web Application that receives username and password via POST request (from Postman).

It receives "email" and "password" as query string from request body.



Parameters:

email: any email address

password: any password string

Finally, it should return message as either "Successful login" or "Invalid login".



Process:

If email is "admin@example.com" and password is "admin1234", it is treated as a valid login; otherwise invalid login.



Example #1:

If you receive a HTTP POST request at path "/", if the valid email and password are submitted, it should return HTTP 200 response.

Request Url: /

Request Method: POST

Request body (input): email=admin@example.com&password=admin1234

Response Status Code: 200

Response body (output):

Successful login




Example #2:

If you receive a HTTP POST request at path "/", if either email or password is incorrect, it should return HTTP 400 response.

Request Url: /

Request Method: POST

Request body (input): email=manager@example.com&password=manager-password

Response Status Code: 400

Response body (output):

Invalid login



Example #3:

If you receive a HTTP POST request at path "/", if neither email and password is submitted, it should return HTTP 400 response.

Request Url: /

Request Method: POST

Request body (input): [empty]

Response Status Code: 400

Response body (output):

Invalid input for 'email'
Invalid input for 'password'



Example #4:

If you receive a HTTP POST request at path "/", if password is not submitted, it should return HTTP 400 response.

Request Url: /

Request Method: POST

Request body (input): email=test@example.com

Response Status Code: 400

Response body (output):

Invalid input for 'password'



Example #5:

If you receive a HTTP POST request at path "/", if email not is submitted, it should return HTTP 400 response.

Request Url: /

Request Method: POST

Request body (input): password=1234

Response Status Code: 400

Response body (output):

Invalid input for 'password'



Example #6:

If you receive a HTTP GET request at path "/", it should return HTTP 200 response.

Request Url: /

Request Method: GET

Response Status Code: 200

Response body (output):

No response





Instructions:

Use custom conventional middleware (with middleware extensions) to handle the post request at path "/"

The "email" and "password" values are mandatory

Return appropriate HTTP status codes based on above examples.

Do not create controllers or any other concept which is not yet covered, to avoid confusion.