using Api.Database;
using Api.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly ApplicationDbContext db;

        public GraphQLController(ApplicationDbContext db) => this.db = db;

        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new AuthorQuery(db)
            };

            var result = await new DocumentExecuter().ExecuteAsync(executionOptions =>
            {
                executionOptions.Schema = schema;
                executionOptions.Query = query.Query;
                executionOptions.OperationName = query.OperationName;
                executionOptions.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
