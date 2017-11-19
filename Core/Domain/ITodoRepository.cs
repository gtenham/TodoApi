using System.Collections.Generic; 
   
namespace TodoApi.Core.Domain
{ 
    public interface ITodoRepository  
    { 
        void SaveTodo(Todo todo); 
        IEnumerable<Todo> GetAllTodos(); 
        Todo GetTodo(long id); 
        void DeleteTodo(Todo todo); 
        void UpdateTodo(Todo todo); 
    } 
}