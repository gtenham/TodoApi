using TodoApi.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; 
using System.Linq; 

namespace TodoApi.Infrastructure.Data
{
    public class TodoRepository : ITodoRepository 
    { 
        private ApplicationContext context; 
        private DbSet<Todo> todoEntity;

        public TodoRepository(ApplicationContext context) 
        {
            this.context = context;
            todoEntity = context.Set<Todo>();
        }

        public void SaveTodo(Todo todo) 
        { 
            context.Entry(todo).State = EntityState.Added; 
            context.SaveChanges(); 
        } 
   
        public IEnumerable<Todo> GetAllTodos() 
        { 
            return todoEntity.AsEnumerable(); 
        } 
   
        public Todo GetTodo(long id) 
        { 
            return todoEntity.SingleOrDefault(s => s.Id == id); 
        } 
        public void DeleteTodo(Todo todo) 
        { 
            //Todo todo = GetTodo(id); 
            todoEntity.Remove(todo); 
            context.SaveChanges(); 
        } 
        public void UpdateTodo(Todo todo) 
        {             
            context.SaveChanges(); 
        }        

    }
}