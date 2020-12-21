using Game2.Figures;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace Game2.GUI
{
    public class Gui
    {
        private readonly Desktop _desktop;
        
        private readonly Figure _figure;
        private readonly CoordinateGrid _coordinateGrid;
        
        private Window _parametersWindow;
        private Window _euclideanWindow;
        private Window _affineWindow;
        private Window _homographyWindow;
        
        public Gui(Figure figure, CoordinateGrid coordinateGrid)
        {
            _figure = figure;
            _coordinateGrid = coordinateGrid;

            CreateParametersWindow();
            CreateEuclideanWindow();
            CreateHomographyWindow();
            CreateAffineWindow();
            
            var grid = new Grid
            {
                RowSpacing = 6,
                ColumnSpacing = 8
            };

            _desktop = new Desktop
            {
                Root = grid
            };
            
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            
            var button = new TextButton
            {
                GridColumn = 0,
                GridRow = 0,
                Text = "Parameters"
            };
            button.Click += (sender, args) =>
            {
                _parametersWindow.ShowModal(_desktop);
            };
            grid.Widgets.Add(button);
            
            button = new TextButton
            {
                GridColumn = 0,
                GridRow = 1,
                Text = "Euclidean"
            };
            button.Click += (sender, args) =>
            {
                _euclideanWindow.ShowModal(_desktop);
            };
            grid.Widgets.Add(button);
            
            button = new TextButton
            {
                GridColumn = 0,
                GridRow = 2,
                Text = "Affine"
            };
            button.Click += (sender, args) =>
            {
                _affineWindow.ShowModal(_desktop);
            };
            grid.Widgets.Add(button);
            
            button = new TextButton
            {
                GridColumn = 0,
                GridRow = 3,
                Text = "Homography"
            };
            button.Click += (sender, args) =>
            {
                _homographyWindow.ShowModal(_desktop);
            };
            grid.Widgets.Add(button);
            
            button = new TextButton
            {
                GridColumn = 0,
                GridRow = 4,
                Text = "Clear"
            };
            button.Click += (sender, args) =>
            {
                _figure.Reset();
                _coordinateGrid.Reset();
                
                CreateParametersWindow();
                CreateEuclideanWindow();
                CreateHomographyWindow();
                CreateAffineWindow();
            };
            grid.Widgets.Add(button);
        }

        public void Render()
        {
            _desktop.Render();
        }

        void CreateEuclideanWindow()
        {
            _euclideanWindow = new Window {
                Title = "Euclidean"
            };
            
            var grid = new Grid();
            
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            for (int i = 0; i < 7; i++)
            {
                grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            var moveParam = AddPointParam(grid, "Move", 0);
            var label = new Label
            {
                Text = "Rotate: ",
                GridColumn = 0,
                GridRow = 2
            };

            var rotateParam = new TextBox
            {
                Text = "0",
                GridColumn = 1,
                GridRow = 2,
                Width = 75
            };
            
            grid.Widgets.Add(label);
            grid.Widgets.Add(rotateParam);
            var rotateCenterParam = AddPointParam(grid, "Rotate Center", 3);

            var button = new TextButton
            {
                Text = "Apply",
                GridRow = 6
            };

            button.Click += (sender, args) =>
            {
                Transform transform = new Transform();
                transform.Move(new Vector3(float.Parse(moveParam.Item1.Text), float.Parse(moveParam.Item2.Text), 0));

                Vector3 rotate = new Vector3(0, 0, float.Parse(rotateParam.Text));
                Vector3 rotateCenter = new Vector3(float.Parse(rotateCenterParam.Item1.Text), float.Parse(rotateCenterParam.Item2.Text), 0);
                
                transform.Rotate(rotate,  _coordinateGrid.Center + rotateCenter);
                
                _figure.ApplyTransform(transform);
            };
            
            grid.Widgets.Add(button);
            
            _euclideanWindow.Content = grid;
        }

        void CreateAffineWindow()
        {
            _affineWindow = new Window
            {
                Title = "Affine"
            };

            Grid grid = new Grid();
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            var r0Param = AddPointParam(grid, "r0", 0);
            var rXParam = AddPointParam(grid, "rX", 2);
            var rYParam = AddPointParam(grid, "rY", 4);

            var button = new TextButton
            {
                Text = "Apply",
                GridRow = 6
            };

            button.Click += (sender, args) =>
            {
                Transform transform = new Transform();

                var r0 = new Vector3(float.Parse(r0Param.Item1.Text), float.Parse(r0Param.Item2.Text), 0);
                var rX = new Vector3(float.Parse(rXParam.Item1.Text), float.Parse(rXParam.Item2.Text), 0);
                var rY = new Vector3(float.Parse(rYParam.Item1.Text), float.Parse(rYParam.Item2.Text), 0);
                transform.Affine(r0, rX, rY);
                
                _figure.ApplyTransform(transform);
                _coordinateGrid.ApplyTransform(transform);
            };
            
            grid.Widgets.Add(button);
            
            _affineWindow.Content = grid;
        }

        void CreateHomographyWindow()
        {
            _homographyWindow = new Window {
                Title = "Homography"
            };
            
            Grid grid = new Grid
            {
                ColumnSpacing = 5,
                RowSpacing = 5
            };
            
            for (int i = 0; i < 4; i++)
            {
                grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
                grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }
            grid.RowsProportions.Add(new Proportion(ProportionType.Auto));

            string[] cols = {" ", "X", "Y", "W"};
            for (int i = 0; i < cols.Length; i++)
            {
                var label = new Label
                {
                    Text = cols[i],
                    GridColumn = i,
                    GridRow = 0,
                    Width = 75,
                    TextAlign = TextAlign.Center
                };
                grid.Widgets.Add(label);
            }
            string[] rows = { "r0:", "rX:", "rY:"};
            for (int i = 0; i < rows.Length; i++)
            {
                var label = new Label
                {
                    Text = rows[i],
                    GridColumn = 0,
                    GridRow = i+1,
                    Width = 75,
                    TextAlign = TextAlign.Center
                };
                grid.Widgets.Add(label);
            }

            var r0X = AddField(grid, "0", 1, 1);
            var r0Y = AddField(grid, "0", 1, 2);
            var rW0 = AddField(grid, "1", 1, 3);
            
            var r1X= AddField(grid, "1", 2, 1);
            var r1Y = AddField(grid, "0", 2, 2);
            var rWX = AddField(grid, "1", 2, 3);
            
            var r2X = AddField(grid, "0", 3, 1);
            var r2Y = AddField(grid, "1", 3, 2);
            var rWY = AddField(grid, "1", 3, 3);
            
            var button = new TextButton
            {
                Text = "Apply",
                GridRow = 5,
            };

            button.Click += (sender, args) =>
            {
                Transform transform = new Transform();
                Vector3 r0 = new Vector3(float.Parse(r0X.Text), float.Parse(r0Y.Text), 0);
                Vector3 rX = new Vector3(float.Parse(r1X.Text), float.Parse(r1Y.Text), 0);
                Vector3 rY = new Vector3(float.Parse(r2X.Text), float.Parse(r2Y.Text), 0);
                Vector3 rW = new Vector3(float.Parse(rWX.Text), float.Parse(rWY.Text), float.Parse(rW0.Text));
                
                transform.Move(-_coordinateGrid.Center);
                transform.Homography(r0, rX, rY, rW);
                transform.Move(_coordinateGrid.Center);
                
                _figure.ApplyTransform(transform);
                _coordinateGrid.ApplyTransform(transform);
            };
            
            grid.Widgets.Add(button);
            
            _homographyWindow.Content = grid;
        }
        
        void CreateParametersWindow()
        {
            _parametersWindow = new Window {
                Title = "Parameters"
            };
            Grid grid = new Grid();
            
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            for (int i = 0; i < 10; i++)
            {
                grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }
            
            AddParam(grid, 0, "L4");
            AddParam(grid, 1, "R1");
            AddParam(grid, 2, "R2");

            _parametersWindow.Content = grid;
        }
        
        private void AddParam(Grid grid, int row, string param)
        {
            var label = new Label
            {
                Id = $"label_{param}",
                Text = $"{param}: ",
                GridColumn = 0,
                GridRow = row
            };

            var textBox = new TextBox
            {
                Id = param,
                Text = typeof(Figure).GetProperty(param)?.GetValue(_figure)?.ToString(),
                GridColumn = 1,
                GridRow = row,
                Width = 75
            };

            textBox.TextChanged += (sender, args) =>
            {
                if (float.TryParse(args.NewValue, out var newValue))
                {
                    typeof(Figure).GetProperty(param)?.SetValue(_figure, newValue);
                }
                else textBox.Text = typeof(Figure).GetProperty(param)?.GetValue(_figure)?.ToString();
            };
            
            grid.Widgets.Add(label);
            grid.Widgets.Add(textBox);
        }
        private (TextBox, TextBox) AddPointParam(Grid grid, string title, int row)
        {
            var label = new Label
            {
                Text = title,
                GridColumn = 0,
                GridRow = row
            };
            grid.Widgets.Add(label);
            label = new Label
            {
                Text = "X: ",
                GridColumn = 0,
                GridRow = row+1,
                Width = 25
            };
            grid.Widgets.Add(label);
            label = new Label
            {
                Text = " Y: ",
                GridColumn = 2,
                GridRow = row+1,
                Width = 25
            };
            grid.Widgets.Add(label);
            
            var textBoxX = new TextBox
            {
                Text = "0",
                GridColumn = 1,
                GridRow = row+1,
                Width = 75
            };
            textBoxX.TextChangedByUser += (sender, args) =>
            {
                if (!float.TryParse(args.NewValue, out var newValue)  && !(sender is null))
                {
                    ((TextBox) sender).Text = args.OldValue;
                }
            };
            grid.Widgets.Add(textBoxX);
            var textBoxY = new TextBox
            {
                Text = "0",
                GridColumn = 3,
                GridRow = row+1,
                Width = 75
            };
            textBoxY.TextChangedByUser += (sender, args) =>
            {
                if (!float.TryParse(args.NewValue, out var newValue) && !(sender is null))
                {
                    ((TextBox) sender).Text = args.OldValue;
                }
            };
            grid.Widgets.Add(textBoxY);
            return (textBoxX, textBoxY);
        }

        private TextBox AddField(Grid grid, string text, int row, int col)
        {
            var box = new TextBox
            {
                Text = text,
                GridColumn = col,
                GridRow = row,
                Width = 75
            };
            grid.Widgets.Add(box);
            return box;
        }
    }
}
