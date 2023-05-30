using System.Drawing.Drawing2D;

namespace V_RGR
{
    public partial class Form1 : Form
    {
        int[,] matrix;
        public Form1()
        {
            InitializeComponent();
        }
        private void FindMinColumnButton_Click(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            int minColumnIndex = FindMinColumn(matrix);
            int[] sortedColumn = GetSortedColumn(matrix, minColumnIndex);
            DisplayColumn(sortedColumn, sortedColumnTextBox);
        }

        private void InverseMatrixButton_Click(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                MessageBox.Show("The matrix should be square.");
                return;
            }

            double[,] inverseMatrix = InvertMatrix(matrix);
            DisplayMatrixDouble(inverseMatrix, inverseMatrixTextBox);
        }

        private void SwapRowsButton_Click(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            SwapRows(matrix);
            DisplayMatrix(matrix, swappedRowsTextBox);
        }

        private void CountZeroValuesButton_Click(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            int[] zeroCounts = CountZeroValues(matrix);
            AppendZeroCountsRow(matrix, zeroCounts);
            DisplayMatrix(matrix, zeroCountsTextBox);
        }

        private void SwapLastRowWithFirstColumnButton_Click(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            SwapLastRowWithFirstColumn(matrix);
            DisplayMatrix(matrix, swappedElementsTextBox);
        }

        private int[,] GenerateIntegerMatrix(int n, int m, int minValue, int maxValue)
        {
            Random random = new Random();
            int[,] matrix = new int[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = random.Next(minValue, maxValue + 1);
                }
            }

            return matrix;
        }

        private void DisplayMatrix(int[,] matrix, TextBox textBox)
        {
            textBox.Text = "";

            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    textBox.Text += matrix[i, j] + "\t";
                }

                textBox.Text += Environment.NewLine;
            }
        }
        private void DisplayMatrixDouble(double[,] matrix, TextBox textBox)
        {
            textBox.Text = "";

            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    textBox.Text += Math.Round(matrix[i, j],3) + "\t";
                }

                textBox.Text += Environment.NewLine;
            }
        }

        private int[] FindRepeatedValues(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            var repeatedValues = matrix.Cast<int>()
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

            return repeatedValues;
        }

        private void DisplayValues(int[] values, TextBox textBox)
        {
            textBox.Text = "Repeated values: ";
            textBox.Text += string.Join(", ", values);
            textBox.Text += Environment.NewLine;
            textBox.Text += "Count: " + values.Length;
        }

        private int FindMinColumn(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            int minColumnIndex = 0;
            int minColumnValue = int.MaxValue;

            for (int j = 0; j < m; j++)
            {
                int columnMin = int.MaxValue;

                for (int i = 0; i < n; i++)
                {
                    if (matrix[i, j] < columnMin)
                    {
                        columnMin = matrix[i, j];
                    }
                }

                if (columnMin < minColumnValue)
                {
                    minColumnValue = columnMin;
                    minColumnIndex = j;
                }
            }

            return minColumnIndex;
        }

        private int[] GetSortedColumn(int[,] matrix, int columnIndex)
        {
            int n = matrix.GetLength(0);
            int[] column = new int[n];

            for (int i = 0; i < n; i++)
            {
                column[i] = matrix[i, columnIndex];
            }

            Array.Sort(column);
            Array.Reverse(column);

            return
            column;
        }

        private void DisplayColumn(int[] column, TextBox textBox)
        {
            textBox.Text = "Sorted column: ";
            textBox.Text += string.Join(", ", column);
        }

        private double[,] InvertMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] inverseMatrix = new double[n, n];

            double[,] augmentedMatrix = new double[n, 2 * n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }

                augmentedMatrix[i, i + n] = 1;
            }

            for (int i = 0; i < n; i++)
            {
                double pivot = augmentedMatrix[i, i];

                for (int j = 0; j < 2 * n; j++)
                {
                    augmentedMatrix[i, j] /= pivot;
                }

                for (int k = 0; k < n; k++)
                {
                    if (k != i)
                    {
                        double coefficient = augmentedMatrix[k, i];

                        for (int j = 0; j < 2 * n; j++)
                        {
                            augmentedMatrix[k, j] -= coefficient * augmentedMatrix[i, j];
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverseMatrix[i, j] = augmentedMatrix[i, j + n];
                }
            }

            return inverseMatrix;
        }

        private void SwapRows(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            for (int i = 0; i < n / 2; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int temp = matrix[i, j];
                    matrix[i, j] = matrix[n - 1 - i, j];
                    matrix[n - 1 - i, j] = temp;
                }
            }
        }

        private int[] CountZeroValues(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int[] zeroCounts = new int[m];

            for (int j = 0; j < m; j++)
            {
                int count = 0;

                for (int i = 0; i < n; i++)
                {
                    if (matrix[i, j] == 0)
                    {
                        count++;
                    }
                }

                zeroCounts[j] = count;
            }

            return zeroCounts;
        }

        private void AppendZeroCountsRow(int[,] matrix, int[] zeroCounts)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int[,] newMatrix = new int[n + 1, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    newMatrix[i, j] = matrix[i, j];
                }
            }

            for (int j = 0; j < m; j++)
            {
                newMatrix[n, j] = zeroCounts[j];
            }

            matrix = newMatrix;
        }

        private void SwapLastRowWithFirstColumn(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);

            for (int j = 0; j < m; j++)
            {
                int temp = matrix[n - 1, j];
                matrix[n - 1, j] = matrix[0, j];
                matrix[0, j] = temp;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void GenerateMatrixButton_Click_1(object sender, EventArgs e)
        {
            if (rowsTextBox.Text.Length < 1 || columnsTextBox.Text.Length < 1) 
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }
            int n = int.Parse(rowsTextBox.Text);
            int m = int.Parse(columnsTextBox.Text);

            matrix = GenerateIntegerMatrix(n, m, 10, 20);
            DisplayMatrix(matrix, matrixTextBox);
        }

        private void FindRepeatedValuesButton_Click_1(object sender, EventArgs e)
        {
            if (matrix == null)
            {
                MessageBox.Show("Please generate a matrix first.");
                return;
            }

            var repeatedValues = FindRepeatedValues(matrix);
            DisplayValues(repeatedValues, repeatedValuesTextBox);
        }
    }
}