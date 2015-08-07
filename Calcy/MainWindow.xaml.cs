using System.Windows;
using MahApps.Metro.Controls;
using Irony.Parsing;
using System;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using Irony.Interpreter;
using System.Windows.Media;

namespace Calcy {
    public partial class MainWindow : MetroWindow {
        public LanguageData language;
        public MathGrammar grammar;
        public Parser parser;
        public ExpressionEvaluator evaluator;

        public MainWindow() {
            InitializeComponent();

            grammar = new MathGrammar();
            language = new LanguageData(grammar);
            parser = new Parser(language);
        }

        private void openSettings(object sender, RoutedEventArgs e) {
            Flyout f = Flyouts.Items[0] as Flyout;
            f.IsOpen = true;
        }

        private void typed(object sender, RoutedEventArgs e) {
            textBlock.Text = "";

            evaluator = new ExpressionEvaluator(grammar);
            foreach (string line in textBox.Text.Split('\n')) {
                RunLine(line);
            }
        } 

        private void RunLine(string l) {
            l = l.Replace("\r", "");
            
            Run output = new Run("");
            output.Foreground = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            try {
                evaluator.ClearOutput();
                evaluator.Evaluate(l);
            } catch (Exception ex) {
                switch(evaluator.App.Status) {
                    case AppStatus.SyntaxError:
                        output.Text = evaluator.App.GetParserMessages().Last().ToString();
                        break;
                    case AppStatus.Crash:
                    case AppStatus.RuntimeError:
                        var scriptEx = evaluator.App.LastException as ScriptException;
                        output.Text = ex.Message;
                        break;
                }
                output.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            if (output.Text.Length == 0) {
                output.Text = evaluator.GetOutput();
            }
            if (output.Text.Length > 0) {
                output.Text = (string.Concat(Enumerable.Repeat(" ", l.Length + 1)) + " // " + output.Text).Replace("\r\n", "");
            }
            textBlock.Inlines.Add(output);
            textBlock.Inlines.Add(new LineBreak());
        }
    }
}
