using System;
using System.Collections.Generic;

public class Test
{
    public static void Main()
    {
        // Sample input for the outliner
        var input = new List<Heading>()
        {
            new Heading(1, "Title1"),
            new Heading(2, "Something"),
            new Heading(2, "Something2"),
            new Heading(3, "SomethingElse"),
            new Heading(2, "Something3"),
            new Heading(3, "SomethingElse2"),
            new Heading(2, "Something4"),
            new Heading(2, "Something5"),
            new Heading(2, "Something6"),
            new Heading(3, "SomethingElse3"),
            new Heading(4, "FinalSomething"),
            new Heading(3, "SomethingElse4"),
            new Heading(3, "SomethingElse5"),
            new Heading(1, "Title2"),
            new Heading(2, "Something"),
            new Heading(2, "Something2"),
            new Heading(1, "Title3"),
            new Heading(2, "Something"),
        };

        var output = Outliner.CreateOutline(input);
        printAnswer(output);

        Console.WriteLine("\n\nPress Any Key To Exit");
        Console.ReadKey();
    }

    // prints the trees of headers, takes multiple tree heads AKA high level headers
    private static void printAnswer(List<Node> nodes)
    {
        if (nodes.Count == 0)
        {
            return;
        }
        foreach (var node in nodes)
        {
            Console.Write('\n');
            for (int i = 1; i < node.NodeHeading.Weight; i++)
            {
                Console.Write(" ");
            }
            Console.Write(node.NodeHeading.Text);
            printAnswer(node.Children);
        }
    }
}

// Static class with one method to create an outline
public static class Outliner
{
    // Returns a list of nodes that outline the headings, multiple nodes means
    // multiple trees
    public static List<Node> CreateOutline(List<Heading> headings)
    {
        // List of top level nodes, will be only list of one if only one top level header
        List<Node> answer = new List<Node>();
        if (headings == null || headings.Count == 0)
        {
            return answer;
        }

        // Stack to keep track of Node level
        Stack<Node> hstack = new Stack<Node>();

        // traverse all headers
        foreach (var heading in headings)
        {
            var node = new Node(heading);

            if (hstack.Count == 0)
            {
                hstack.Push(node);
                continue;
            }

            Node popped = null;
            while (hstack.Count > 0 && hstack.Peek().NodeHeading.Weight >= heading.Weight)
            {
                popped = hstack.Pop();
            }

            // checks if top level heading was popped to add to answer
            if (hstack.Count == 0 && popped != null)
            {
                // resets the stack with a new tree head
                answer.Add(popped);
                hstack.Push(node);
            }
            else
            {
                // Adds node to the child of the top of the stack
                hstack.Peek().Children.Add(node);
                hstack.Push(node);
            }
        }

        // get the top level node if not already in answer
        if (hstack.Count != 0)
        {
            while (hstack.Count > 1)
            {
                hstack.Pop();
            }
            answer.Add(hstack.Pop());
        }

        return answer;
    }
}

// Heading class with weight and text
public class Heading
{
    public int Weight { get; private set; }
    public string Text { get; private set; }

    public Heading(int weight, string text)
    {
        if (String.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException("text");
        }
        if (weight < 0)
        {
            throw new ArgumentException("weight");
        }

        Weight = weight;
        Text = text;
    }
}

// Tree node class with heading and it's childrenw with lower weights
public class Node
{
    public List<Node> Children { get; private set; }
    public Heading NodeHeading { get; private set; }

    public Node(Heading heading)
    {
        if (heading == null)
        {
            throw new ArgumentNullException("heading");
        }
        NodeHeading = heading;
        Children = new List<Node>();
    }
}