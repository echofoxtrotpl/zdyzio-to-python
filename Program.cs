using Antlr4.Runtime;
using ZdyzioToPython;
using ZdyzioToPython.Content;

//var fileName = @"Inputs/test1.txt";

//var fileContents = File.ReadAllText(fileName);

//var inputStream = new AntlrInputStream(fileContents);
//var zdyzioLexer = new ZdyzioLexer(inputStream);
//var commonTokenStream = new CommonTokenStream(zdyzioLexer);
//var zdyzioParser = new ZdyzioParser(commonTokenStream);
//var zdyzioContext = zdyzioParser.program();
//var visitor = new ZdyzioVisitor();

//var result = visitor.Visit(zdyzioContext);

//File.WriteAllText(@"result.py",result);

//Console.WriteLine(result);

for (int i = 1; i < 6; i++)
{
    var fileName = $@"Inputs/test{i}.txt";
    var fileContents = File.ReadAllText(fileName);

    var inputStream = new AntlrInputStream(fileContents);
    var zdyzioLexer = new ZdyzioLexer(inputStream);
    var commonTokenStream = new CommonTokenStream(zdyzioLexer);
    var zdyzioParser = new ZdyzioParser(commonTokenStream);
    var zdyzioContext = zdyzioParser.program();
    var visitor = new ZdyzioVisitor();

    var result = visitor.Visit(zdyzioContext);

    File.WriteAllText(@$"result{i}.py", result);
    Console.WriteLine($"Result {i}\n");
    Console.WriteLine(result);
    Console.WriteLine();
}
