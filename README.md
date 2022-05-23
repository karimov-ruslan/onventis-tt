Technical task

Create ASP.NET Core application with the following acceptance criteria.
1. Application has an endpoint which accepts request of structure:

class Invoice 
string Description;
DateTime DueDate;
string Supplier;
List<InvoiceLine> Lines;

class InvoiceLine 
string Description;
double Price;
int Quantity;

  
2. Endpoint shoud be authenticated by JWT.
  
3. As a result of request execution
the data should be saved into SQLite database (using Entity Framework)
the request data is sent to the second application by RabbitMQ (cloud version is possible at https://www.cloudamqp.com/)
4. .NET 5 Console application subscribes to the RabbitMQ, receives the message from first application and writes received message to the Console.
  
Nice to have:
Postman collection to test application (auth + invoice sending)
use Serilog for logging
add dockerfile with two stages: build + runtime
publish source code to GitHub
build applications using GitHub Actions
