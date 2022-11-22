using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String Id = Request.Query["Id"];
            try
            {
                String connecionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connecionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE Id=@Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.Id = "" + reader.GetInt32(0);
                                clientInfo.name = "" + reader.GetString(1);
                                clientInfo.email = "" + reader.GetString(2);
                                clientInfo.phone = "" + reader.GetString(3);
                                clientInfo.address = "" + reader.GetString(4);

                                clientInfo.created_at = reader.GetDateTime(5).ToString();

                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

            }
        }
        public void OnPost()
        {
            clientInfo.Id = Request.Form["Id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.Id.Length == 0 ||  clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
                    clientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are reuired";
                return;
            }
            //save the new client into a database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients " +
                        "SET name=@name, email=@email, phone=@phone, address=@address " +
                        "WHERE Id=@Id ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@Id", clientInfo.Id);


                        command.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}