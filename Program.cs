using Mtrx;
using BlockMatrx;

BlockMatrix z = new(new List<List<double>>() {
  new() {2, 2, 3, 4},
  new() {4, 5, 6, 1},
  new() {2, 3, 4, 5},
  new() {5, 2, 1, 3},
});

BlockMatrix zz = new(new List<List<double>>() {
  new() {7, 8, 2, 3},
  new() {9, 3, 4, 1},
  new() {2, 3, 3, 4},
  new() {5, 1, 2, 6},
});

BlockMatrix res = z * zz;

res.ShowMatrix();

