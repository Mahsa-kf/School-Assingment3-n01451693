﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using School.Models;

namespace School.Controllers
{
    public class TeacherDataController : ApiController
    {
        //internal function that will be used by public functions
        private SchoolDbContext dbContext = new SchoolDbContext();


        /// <summary>
        /// Delete a teacher information for a specific teacherId
        /// </summary>
        /// <param name="id">The id of the teacher</param>
        /// <example>POST /api/teacherData/DeleteTeacher/2</example>
        /// <returns>teacher information</returns>
        [HttpPost]
        public void AddTeacher([FromBody] Teacher newTeacher)
        {
            //Create an instance of DB connection
            MySqlConnection Conn = dbContext.AccessDatabase();

            //Open the connection
            Conn.Open();

            //Establish a new command 
            MySqlCommand cmd = Conn.CreateCommand();

            //Bulid the SQL query
            cmd.CommandText = "INSERT INTO teachers(teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";


            cmd.Parameters.AddWithValue("@teacherfname", newTeacher.Teacherfname);
            cmd.Parameters.AddWithValue("@teacherlname", newTeacher.Teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", newTeacher.Employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", newTeacher.Hiredate);
            cmd.Parameters.AddWithValue("@salary", newTeacher.Salary);
            cmd.Prepare();

            //Executing the sql query 
            cmd.ExecuteNonQuery();

            Conn.Close();
        }


        /// <summary>
        /// Delete a teacher information for a specific teacherId
        /// </summary>
        /// <param name="id">The id of the teacher</param>
        /// <example>POST /api/teacherData/DeleteTeacher/2</example>
        /// <returns>teacher information</returns>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of DB connection
            MySqlConnection Conn = dbContext.AccessDatabase();

            //Open the connection
            Conn.Open();

            //Establish a new command 
            MySqlCommand cmd = Conn.CreateCommand();

            //Bulid the SQL query
            cmd.CommandText = "DELETE FROM Teachers WHERE teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            //Executing the sql query 
            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        //This Controller Will access the teacher table of our blog database.
        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/teacherData/GetTeachers</example>
        /// <returns>
        /// A list of teachers
        /// </returns>
        [HttpGet]
        [Route("api/teacherData/GetTeachers")]
        public IEnumerable<Teacher> GetTeachers()
        {
            //SQL query
            string sqlQuery = "SELECT * FROM Teachers";

            //Execute the SQL query and get the list of teachers
            IEnumerable<Teacher> result = GetTeachersByQuery(sqlQuery);

            //Return the result
            return result;
        }

        /// <summary>
        /// Return a teacher information for a specific teacherId
        /// </summary>
        /// <param name="id">The id of the teacher</param>
        /// <example>GET api/teacherData/GetTeacher/{id}</example>
        /// <returns>teacher information</returns>
        [HttpGet]
        [Route("api/teacherData/GetTeacher/{id}")]
        public Teacher GetTeacher(int id)
        {
            //SQL query
            string sqlQuery = "SELECT * FROM Teachers WHERE teacherid = " + id;

            //Execute the SQL query and get the teacher
            IEnumerable<Teacher> result = GetTeachersByQuery(sqlQuery);

            //Convert IEnumerable<Teacher> whith item to Teacher and return it
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Execute the input query and return the result
        /// </summary>
        /// <param name="query">The SQL query</param>
        /// <returns>list of teachers</returns>        
        private List<Teacher> GetTeachersByQuery(string query)
        {
            //Create an empty list of Teachers
            List<Teacher> teachers = new List<Teacher> { };

            MySqlConnection Conn = dbContext.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = query;

            //Gather Result Set of Query into a variable
            MySqlDataReader resultSet = cmd.ExecuteReader();

            //Loop Through Each Row the Result Set
            while (resultSet.Read())
            {
                //creating an instance of teacher model
                Teacher newTeacher = new Teacher();

                //set the values of the new Teacher object
                newTeacher.Teacherid = (int)resultSet["teacherid"];
                newTeacher.Teacherfname = (string)resultSet["teacherfname"];
                newTeacher.Teacherlname = (string)resultSet["teacherlname"];
                newTeacher.Employeenumber = (string)resultSet["employeenumber"];
                newTeacher.Hiredate = (DateTime)resultSet["hiredate"];
                newTeacher.Salary = (decimal)resultSet["salary"];

                //Add the new teacher to the List
                teachers.Add(newTeacher);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teachers
            return teachers;
        }

        /// <summary>
        /// Updates an teacher on the MySQL Database.
        /// </summary>
        /// <param name="teacher">An object with fields that map to the columns of the author's table.</param>

        [HttpPost]
        public void UpdateTeacher([FromBody] Teacher teacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = dbContext.AccessDatabase();

            //Debug.WriteLine(TeacherInfo.Teacherfname);

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update Teachers set teacherfname=@Teacherfname, teacherlname=@Teacherlname, employeenumber=@Employeenumber, hiredate=@Hiredate,  salary=@Salary  where teacherid=@Teacherid";
            cmd.Parameters.AddWithValue("@Teacherfname", teacher.Teacherfname);
            cmd.Parameters.AddWithValue("@Teacherlname", teacher.Teacherlname);
            cmd.Parameters.AddWithValue("@Employeenumber", teacher.Employeenumber);
            cmd.Parameters.AddWithValue("@Hiredate", teacher.Hiredate);
            cmd.Parameters.AddWithValue("@Salary", teacher.Salary);
            cmd.Parameters.AddWithValue("@Teacherid", teacher.Teacherid);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }
    }
}


