using Antlr4.Runtime;
using ZdyzioToPython;
using ZdyzioToPython.Content;

for (int i = 1; i < 6; i++)
{
    var fileName = $@"Inputs/test{i}.txt";
    var fileContents = File.ReadAllText(fileName);

    var inputStream = new AntlrInputStream(fileContents);
    var zdyzioLexer = new ZdyzioLexer(inputStream);
    var commonTokenStream = new CommonTokenStream(zdyzioLexer);
    var zdyzioParser = new ZdyzioParser(commonTokenStream);
    
    zdyzioParser.RemoveErrorListeners();

    zdyzioParser.AddErrorListener(new ZdyzioErrorListener());

    var zdyzioContext = zdyzioParser.program();
    var visitor = new ZdyzioVisitor();

    try
    {
        Console.WriteLine($"Result {i}\n");
        var result = visitor.Visit(zdyzioContext);
        File.WriteAllText(@$"result{i}.py", result);
        Console.WriteLine(result);
    }
    catch (Exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ZdyzioErrorListener.ErrorMessages.First());
        Console.ForegroundColor = ConsoleColor.Black;
        ZdyzioErrorListener.ErrorMessages.Clear();
    }
}
