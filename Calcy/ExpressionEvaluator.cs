using Irony.Interpreter;
using Irony.Parsing;
using System.Collections.Generic;

namespace Calcy {
    public class ExpressionEvaluator {
        public MathGrammar Grammar { get; private set; }
        public Parser Parser { get; private set; }
        public LanguageData Language { get; private set; }
        public LanguageRuntime Runtime { get; private set; }
        public ScriptApp App { get; private set; }

        public IDictionary<string, object> Globals {
            get { return App.Globals; }
        }

        //Default constructor, creates default evaluator 
        public ExpressionEvaluator() : this(new MathGrammar()) {
        }

        //Default constructor, creates default evaluator 
        public ExpressionEvaluator(MathGrammar grammar) {
            Grammar = grammar;
            Language = new LanguageData(Grammar);
            Parser = new Parser(Language);
            Runtime = Grammar.CreateRuntime(Language);
            App = new ScriptApp(Runtime);

            Globals.Add("null", Runtime.NoneValue);
            Globals.Add("true", true);
            Globals.Add("false", false);
        }

        public object Evaluate(string script) {
            var result = App.Evaluate(script);
            return result;
        }

        public object Evaluate(ParseTree parsedScript) {
            var result = App.Evaluate(parsedScript);
            return result;
        }

        //Evaluates again the previously parsed/evaluated script
        public object Evaluate() {
            return App.Evaluate();
        }

        public void ClearOutput() {
            App.ClearOutputBuffer();
        }
        public string GetOutput() {
            return App.GetOutput();
        }

    }//class
}