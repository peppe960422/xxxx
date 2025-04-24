using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO;
using MySqlConnector;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.CSharp;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace xxxx
{
    public partial class Form1 : Form
    {
        
        string connectionString = "server=127.0.0.1;uid=root;pwd=System1!";
        CodeCompileUnit targetUnit = new CodeCompileUnit(); 
        CodeNamespace myNamespace;

        List<MySqlEntity> entities = new List<MySqlEntity>();
        List<ForeingKey> FKs = new List<ForeingKey>();  
        List<CodeTypeDeclaration> MyClasses = new List<CodeTypeDeclaration>();

        List<Assembly> assemblies = new List<Assembly>();
        public Form1()
        {
            InitializeComponent(); 
            myNamespace = new CodeNamespace("DynamicNamespace");
                    myNamespace.Imports.Add(new CodeNamespaceImport("System"));
                   

                    //CodeCompileUnit compileUnit = new CodeCompileUnit();
                    targetUnit.Namespaces.Add(myNamespace);
         
        }
        public CodeTypeDeclaration CrateClass(string ClassName, CodeNamespace t_namespace) 
        {
            //targetUnit = new CodeCompileUnit();
            //CodeNamespace samples = new CodeNamespace("xxxx");
            //samples.Imports.Add(new CodeNamespaceImport("System"));
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration(ClassName);


            targetClass.IsClass = true;
            targetClass.TypeAttributes =
                TypeAttributes.Public ;
            t_namespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(t_namespace);
            CSharpCodeProvider provider = new CSharpCodeProvider();
            StringWriter writer = new StringWriter();
            provider.GenerateCodeFromCompileUnit(targetUnit, writer, new CodeGeneratorOptions());

            // Print out the generated code
            string generatedCode = writer.ToString();
           // Console.WriteLine(generatedCode);
            //File.WriteAllText($@"..\..\..\xxxx\{ClassName}.cs", targetClass.ToString());
            return targetClass;
            //GenerateCSharpCode($@"..\..\..\xxxx\{ClassName}.cs");



        }
        public string GenerateCSharpCode(/*CodeTypeDeclaration typeDeclaration*/)
        {
            using (var provider = new CSharpCodeProvider())
            {
                using (var writer = new StringWriter())
                {
                 

                    provider.GenerateCodeFromCompileUnit(targetUnit, writer, new CodeGeneratorOptions());
                  //myNamespace.Types.Add(typeDeclaration);


                    return writer.ToString();
                }
            }
        }
        public Assembly GenerateCodeInRunTime(CodeTypeDeclaration typeDeclaration, CodeNamespace t_namespace)
        {
            using (var provider = new CSharpCodeProvider())
            {
                using (var writer = new StringWriter())
                {
                   
                    //CodeNamespace myNamespace = new CodeNamespace("DynamicNamespace");
                    //myNamespace.Imports.Add(new CodeNamespaceImport("System"));
                    //myNamespace.Types.Add(typeDeclaration);

                   CodeCompileUnit compileUnit = new CodeCompileUnit();
                    compileUnit.Namespaces.Add(t_namespace);

                    provider.GenerateCodeFromCompileUnit(compileUnit, writer, new CodeGeneratorOptions());
                    CompilerParameters parameters = new CompilerParameters
                    {
                        GenerateInMemory = true,
                        GenerateExecutable = false 
                    };
                    parameters.ReferencedAssemblies.Add("System.dll");
                    parameters.ReferencedAssemblies.Add("System.Core.dll");
                    parameters.ReferencedAssemblies.Add("System.Data.dll");
                    CompilerResults results = provider.CompileAssemblyFromDom(parameters, compileUnit);
                    if (results.Errors.HasErrors)
                    {
                        foreach (CompilerError error in results.Errors)
                        {
                            Console.WriteLine($"Fehler: {error.ErrorText}");
                        }
                    }
                    return results.CompiledAssembly;


                }
            }
          
        
        
        
        }
    
        public void AddField(Type type, string name , MemberAttributes modifier,CodeTypeDeclaration klasse)
        {
           
            CodeMemberField widthValueField = new CodeMemberField();
            widthValueField.Attributes = modifier;
            widthValueField.Name = name;
            widthValueField.Type = new CodeTypeReference(type);

            klasse.Members.Add(widthValueField);

  
        }
        public void AddProperties(Type type, string name, 
                                    MemberAttributes modifier,string nameOfRefField, 
                                    CodeTypeDeclaration Klasse, bool hasGet,bool hasSet 
                                    /*,bool getSubRoutine, CodeBinaryOperatorType operationForSubRoutine*/)
        {
            // Declare the read-only Width property.
            CodeMemberProperty Property = new CodeMemberProperty();
            Property.Attributes = modifier;
             
            Property.Name = name;
            Property.HasGet = hasGet;
            Property.HasSet = hasSet;
            Property.Type = new CodeTypeReference(type);
          
            Property.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), nameOfRefField)));
            Property.SetStatements.Add( new CodeSnippetExpression($"{nameOfRefField}=value"));
            Klasse.Members.Add(Property);

            // Declare the read-only Height property.
          
            // Declare the read only Area property.
            //CodeMemberProperty areaProperty = new CodeMemberProperty();
            //areaProperty.Attributes =
            //    MemberAttributes.Public | MemberAttributes.Final;
            //areaProperty.Name = "Area";
            //areaProperty.HasGet = true;
            //areaProperty.Type = new CodeTypeReference(typeof(System.Double));
            //areaProperty.Comments.Add(new CodeCommentStatement(
            //    "The Area property for the object."));
          
            //// Create an expression to calculate the area for the get accessor
            //// of the Area property.
            //CodeBinaryOperatorExpression areaExpression =
            //    new CodeBinaryOperatorExpression(
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "widthValue"),
            //    CodeBinaryOperatorType.Multiply,
            //    new CodeFieldReferenceExpression(
            //    new CodeThisReferenceExpression(), "heightValue"));
            //areaProperty.GetStatements.Add(
            //    new CodeMethodReturnStatement(areaExpression));
            //targetClass.Members.Add(areaProperty);
        }

        private void buttonExecuteGenerationClass_Click<T>(object sender, EventArgs e)
        {
            //CCodeGenerator Generator = new CCodeGenerator();
            //Generator.CreateNamespace();
            //Generator.CreateImports();

            // Query SQL
            string queryEntity = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'mydb';";
            string result = "";

            // Connessione al database
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(queryEntity, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = ((string)reader["TABLE_NAME"]);
                            entities.Add(new MySqlEntity(name));
                            MyClasses.Add(CrateClass(entities[entities.Count-1].NameEntity,myNamespace));
                            Assembly ass = GenerateCodeInRunTime(MyClasses[MyClasses.Count - 1], myNamespace);
                            string typeName = MyClasses[MyClasses.Count-1].Name;
                            assemblies.Add(ass);
                            Type t = ass.GetType($"DynamicNamespace.{typeName}");
                            MySqlToCSharpTypeConverter.mySqlDatatypes.Add(name.ToUpper(),t);
                        }
                    }
                }conn.Close();        
            }
            string queryKeys = @"
            SELECT 
                TABLE_NAME ,
                COLUMN_NAME ,
                CONSTRAINT_NAME ,
                REFERENCED_TABLE_NAME ,
                REFERENCED_COLUMN_NAME
            FROM 
                INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE 
                TABLE_SCHEMA = 'mydb'
                AND REFERENCED_TABLE_NAME IS NOT NULL;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(queryKeys, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader["TABLE_NAME"].ToString();
                        string columnName = reader["COLUMN_NAME"].ToString();                       
                        string referencedTable = reader["REFERENCED_TABLE_NAME"].ToString();
                        string referencedColumn = reader["REFERENCED_COLUMN_NAME"].ToString();
                        FKs.Add(new ForeingKey(tableName, columnName, referencedTable, referencedColumn));
                        Debug.Write (tableName+" "+ columnName+" "+referencedColumn+" "+referencedTable);
                    }
                }
                connection.Close(); 
            }


            foreach (MySqlEntity entity in entities)
            {
                Debug.WriteLine("*** "+entity.NameEntity);
            
            }
                foreach (MySqlEntity entity in entities)
            {


                string tableName = entity.NameEntity;

                string queryFields = $@"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = 'mydb'
                AND TABLE_NAME = @tableName;";

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryFields, conn))
                    {
                        cmd.Parameters.AddWithValue("@tableName", tableName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nameAttribute = (string)reader["COLUMN_NAME"];
                                Type t = null;
                                foreach (ForeingKey f in FKs) 
                                { 
                                
                                    if (nameAttribute == f.Colonna) 
                                    {

                                            t = MySqlToCSharpTypeConverter.Convert(f.RefTable);
                                             entity.AddAttribute(nameAttribute, t);

                                    }
                                
                                
                                }
                          
                                if (t == null ) { entity.AddAttribute(nameAttribute, MySqlToCSharpTypeConverter.Convert((string)reader["DATA_TYPE"])); }

                            }
                        }
                    }
                }
            

                ausgabeTextBox.Text += entity.NameEntity.ToUpper() + Environment.NewLine;
                foreach (var v in entity.attributesDictionary) 
                {

                    if (v.Value != null)
                    {
                        //if (v.Key.Contains("id") && v.Key != "id") 
                        //{
                        //    string[] parts = v.Key.Split('_');
                        //    string objName = parts[1];                       


                        //}
                        var currentClass = MyClasses.FirstOrDefault(c => c.Name == entity.NameEntity);
                        if (currentClass != null)
                        {
                            AddField(v.Value, "field_" + v.Key, MemberAttributes.Private, currentClass);
                            AddProperties(type :v.Value, 
                                          name : v.Key, 
                                          modifier :MemberAttributes.Public, 
                                          nameOfRefField: "field_" + v.Key,
                                          Klasse: currentClass,
                                          hasGet:  true, 
                                          hasSet : true);
                        }
                      ;
                       

                        ausgabeTextBox.Text += v.Key +" = "+ v.Value.ToString() + Environment.NewLine;
                    }
                
                }
                //foreach (var type in MySqlToCSharpTypeConverter.mySqlDatatypes) { Debug.WriteLine(type.Value.ToString()); }   


                File.WriteAllText($@"..\..\..\xxxx\DynamicNameSpace.cs",  GenerateCSharpCode(/*currentClass*/));

            }

            int i = 0;
                foreach (Assembly a in assemblies) { 
                Type Typobj = a.GetType($"DynamicNamespace.{entities[i].NameEntity}");

                T obj = (T)Activator.CreateInstance(Typobj);


                PropertyInfo [] listprops = obj.GetType().GetProperties();

                foreach (var v  in listprops) 
                {
                    ausgabeTextBox.Text += ( v.GetType().ToString() + "=" + v.GetValue(obj).ToString() + Environment.NewLine);
                
                }
                i++; 
            }


        }   

        Type FindType (string strg) 
        {

            string[] strngPrts = strg.Split('i','d','_');
            if (strngPrts != null) 
            {
                string str = String.Empty;
                foreach (string strng in strngPrts) { str += strng; }
               

                    return MySqlToCSharpTypeConverter.Convert(str);
                


            }


            return typeof(string);
        }

    }

   public  class MySqlEntity 
    { 

        public Type type { get; set; }
        public string NameEntity {  get; set; }
       public  Dictionary<string, Type> attributesDictionary = new Dictionary<string, Type>();
        public MySqlEntity(string name)
        {
            this.NameEntity = name; 
        }

        public void AddAttribute (string nameAttribute ,Type t) 
        {
            if (!attributesDictionary.ContainsKey(nameAttribute)) 
            {     attributesDictionary.Add(nameAttribute, t);}
        
        }




    }

    public class ForeingKey 
    { 
       public  string Table {  get; set; } 
        public string Colonna { get; set; }
        public string RefTable { get; set; }    
        public string RefColonna { get; set; }


        public ForeingKey(string table , string colonna , string refTable, string refColonna)
        {
            this.Table = table;
            this.Colonna = colonna; 
            this.RefTable = refTable;   
            this.RefColonna = refColonna;   

        }

    
    
    }
    public class CCodeGenerator
    {
        CodeNamespace mynamespace;
        CodeTypeDeclaration myclass;
        CodeCompileUnit myassembly;
        public void CreateNamespace()
        {
            mynamespace = new CodeNamespace("myDynamoNamespace");
        }
        public void CreateImports()
        {
            mynamespace.Imports.Add(new CodeNamespaceImport("System"));
            mynamespace.Imports.Add(new CodeNamespaceImport("System.Drawing"));
            mynamespace.Imports.Add(new CodeNamespaceImport("System.Windows.Forms"));
        }
    }
    public static class MySqlToCSharpTypeConverter 
    
    {
        static public Dictionary<string, Type> mySqlDatatypes = new Dictionary<string, Type>
    {
       // Numeric Types
        { "TINYINT", typeof(sbyte) },
        { "SMALLINT", typeof(short) },
        { "MEDIUMINT", typeof(int) },
        { "INT", typeof(int) },
        { "INTEGER", typeof(int) },
        { "BIGINT", typeof(long) },
        { "FLOAT", typeof(float) },
        { "DOUBLE", typeof(double) },
        { "DECIMAL", typeof(decimal) },
        { "DEC", typeof(decimal) },
        { "NUMERIC", typeof(decimal) },

        // Date and Time Types
        { "DATE", typeof(DateTime) },
        { "DATETIME", typeof(DateTime) },
        { "TIMESTAMP", typeof(DateTime) },
        { "TIME", typeof(TimeSpan) },
        { "YEAR", typeof(int) },

        // String Types
        { "CHAR", typeof(string) },
        { "VARCHAR", typeof(string) },
        { "TINYTEXT", typeof(string) },
        { "TEXT", typeof(string) },
        { "MEDIUMTEXT", typeof(string) },
        { "LONGTEXT", typeof(string) },
        { "BINARY", typeof(byte[]) },
        { "VARBINARY", typeof(byte[]) },
        { "TINYBLOB", typeof(byte[]) },
        { "BLOB", typeof(byte[]) },
        { "MEDIUMBLOB", typeof(byte[]) },
        { "LONGBLOB", typeof(byte[]) },
        { "ENUM", typeof(string) },
        { "SET", typeof(string) },

        // Boolean Type
        { "BOOLEAN", typeof(bool) },
        { "BOOL", typeof(bool) },

      
    };

        public static Type Convert(string mysqlType)
        {
            if (mySqlDatatypes.TryGetValue(mysqlType.ToUpper(), out Type dotNetType))
            {
                return dotNetType;
            }

        
            return typeof(object);
        }





    }

    }

