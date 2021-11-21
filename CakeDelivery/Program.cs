using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeDelivery
{
    class Program
    {
        static void Main(string[] args)
        {
            var pathToNorwich = new Graph().GetShortestPathsBetweenTowns("Grimsby", "Norwich", out var norwichDistance);

            var pathToHogsfeet = new Graph().GetShortestPathsBetweenTowns("Norwich", "Hogsfeet", out double hogsfeetTime);
           
            var pathToMatlock = new Graph().GetShortestPathsBetweenTowns("Norwich", "Matlock", out double matlockTime);

            string pathToChester;

            if (hogsfeetTime < matlockTime)
            {
                var newPathToMatlock = new Graph().GetShortestPathsBetweenTowns("Hogsfeet", "Matlock", out double newMatlockTime);

                pathToHogsfeet = $"{pathToHogsfeet},{RemoveFirstPlace(newPathToMatlock)}";

                pathToChester = new Graph().GetShortestPathsBetweenTowns("Matlock", "Chester", out var chesterTime);

                pathToChester = $"{pathToHogsfeet},{RemoveFirstPlace(pathToChester)}";
            }
            else
            {
                var newPathToHogsfeet = new Graph().GetShortestPathsBetweenTowns("Matlock", "Hogsfeet", out var newHogsfeetTime);

                pathToMatlock = $"{pathToMatlock},{RemoveFirstPlace(newPathToHogsfeet)}";

                pathToChester = new Graph().GetShortestPathsBetweenTowns("Hogsfeet", "Chester", out var chesterTime);

                pathToChester = $"{pathToMatlock},{RemoveFirstPlace(pathToChester)}";
            }


            Console.WriteLine($"{pathToNorwich},{RemoveFirstPlace(pathToChester)}");
        }


        private static string RemoveFirstPlace(string path)
        {
            return path.Substring(path.IndexOf(',') + 1);
        }
    }

    class Graph
    {
        private List<Node> Nodes = new()
        {
            new Node
            {
                Name = "Norwich",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Cherrytown", Time = 0.7
                    },
                    new()
                    {
                        Name = "Middletown", Time = 0.5
                    },
                    new()
                    {
                        Name = "Hillford", Time = 2.4
                    },
                }
            },
            new Node
            {
                Name = "Cherrytown",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Norwich", Time = 0.7
                    },
                    new()
                    {
                        Name = "Grimsby", Time = 2.6
                    },
                    new()
                    {
                        Name = "Hillford", Time = 2.2
                    },
                }
            },
            new Node
            {
                Name = "Grimsby",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Cherrytown", Time = 2.6
                    },
                    new()
                    {
                        Name = "Hillford", Time = 1.7
                    }
                }
            },
            new Node
            {
                Name = "Hillford",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Norwich", Time = 2.4
                    },
                    new()
                    {
                        Name = "Cherrytown", Time = 2.2
                    },
                    new()
                    {
                        Name = "Middletown", Time = 2.0
                    },
                    new()
                    {
                        Name = "Grimsby", Time = 1.7
                    },
                    new()
                    {
                        Name = "Tarmsworth", Time = 1.7
                    },
                }
            },
            new Node
            {
                Name = "Middletown",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Norwich", Time = 0.5
                    },
                    new()
                    {
                        Name = "Hogsfeet", Time = 1.5
                    },
                    new()
                    {
                        Name = "Hillford", Time = 2.0
                    },
                    new()
                    {
                        Name = "Murrayfield", Time = 2.0
                    },
                    new()
                    {
                        Name = "Tarmsworth", Time = 1.5
                    },
                }
            },
            new Node
            {
                Name = "Tarmsworth",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Middletown", Time = 1.5
                    },
                    new()
                    {
                        Name = "Hillford", Time = 1.7
                    },
                    new()
                    {
                        Name = "Murrayfield", Time = 2.1
                    }
                }
            },
            new Node
            {
                Name = "Murrayfield",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Middletown", Time = 2.0
                    },
                    new()
                    {
                        Name = "Tarmsworth", Time = 2.1
                    },
                    new()
                    {
                        Name = "Hogsfeet", Time = 1.8
                    },
                    new()
                    {
                        Name = "Matlock", Time = 0.3
                    }
                }
            },
            new Node
            {
                Name = "Hogsfeet",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Murrayfield", Time = 1.8
                    },
                    new()
                    {
                        Name = "Chester", Time = 2.0
                    }
                }
            },
            new Node
            {
                Name = "Matlock",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Murrayfield", Time = 0.3
                    },
                    new()
                    {
                        Name = "Chester", Time = 1.4
                    }
                }
            },
            new Node
            {
                Name = "Chester",
                IsChecked = false,
                ConnectedNodes = new List<Node>()
                {
                    new()
                    {
                        Name = "Matlock", Time = 1.4
                    },
                    new()
                    {
                        Name = "Hogsfeet", Time = 2.0
                    }
                }
            }
        };

        private List<Path> GetShortestPaths(string startNode)
        {
            var paths = Nodes.Select(node => new Path() { Node = node.Name, ShortestDistance = int.MaxValue }).ToList();

            paths.Find(p => p.Node.ToUpper().Equals(startNode.ToUpper())).ShortestDistance = 0;

            while (Nodes.Exists(n => n.IsChecked == false))
            {
                var uncheckedNodes = Nodes.FindAll(n => n.IsChecked == false);

                var nextPath = paths.OrderBy(p => p.ShortestDistance)
                    .First(p => uncheckedNodes.Contains(GetByName(p.Node)));

                var nextNode = GetByName(nextPath.Node);

                foreach (var node in nextNode.ConnectedNodes)
                {
                    var currentNode = GetByName(node.Name);

                    if (currentNode.IsChecked) continue;

                    var currentPath = paths.Find(p => p.Node == currentNode.Name);
                    var newDistance = node.Time + nextPath.ShortestDistance;

                    if (!(currentPath?.ShortestDistance > newDistance)) continue;

                    currentPath.ShortestDistance = newDistance;
                    currentPath.PreviousNode = nextPath.Node;
                }

                nextNode.IsChecked = true;
            }

            return paths;
        }

        public string GetShortestPathsBetweenTowns(string startTown, string stopTown, out double distance)
        {
            var generatedPaths = GetShortestPaths(startTown);

            var startPath = generatedPaths.Find(p => p.Node.Equals(stopTown));

            var paths = new List<string> { startPath.Node };

            distance = startPath.ShortestDistance;

            while (startPath.PreviousNode != null)
            {
                startPath = generatedPaths.Find(p => p.Node.Equals(startPath.PreviousNode));
                paths.Add(startPath.Node);
            }

            paths.Reverse();

            return string.Join(',', paths);
        }

        private Node GetByName(string name)
        {
            return Nodes.Find(n => n.Name.ToUpper().Equals(name.Trim().ToUpper()));
        }
    }

    class Node
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public List<Node> ConnectedNodes { get; set; }
        public double Time { get; set; }
    }

    class Path
    {
        public string Node { get; set; }
        public double ShortestDistance { get; set; }
        public string PreviousNode { get; set; }
    }
}