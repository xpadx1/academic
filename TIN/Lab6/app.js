const express = require("express");
const fs = require("fs");
const sqlite3 = require("sqlite3").verbose();
const path = require("path");

const app = express();
const db = new sqlite3.Database("./lab6.db");

app.use(express.urlencoded({ extended: false }));
app.use(express.static("public"));

function render(viewName, data = {}) {
  const filePath = path.join(__dirname, "public", viewName);
  let html = fs.readFileSync(filePath, "utf8");

  for (const key in data) {
    const regex = new RegExp(`{{\\s*${key}\\s*}}`, "g");
    html = html.replace(regex, data[key]);
  }

  return html;
}

// Helper to render table rows
function renderRows(rows) {
  return rows
    .map(
      (r) => `
    <tr>
      <td>${r.id}</td>
      <td>${r.dict_name}</td>
      <td>${r.title}</td>
      <td>${r.description}</td>
      <td>${r.quantity}</td>
      <td>${new Date(r.created_at).toLocaleString()}</td>
      <td><a class="btn" href="/edit/${r.id}">Edit</a></td>
      <td><a class="btn-danger" href="/delete/${r.id}">Delete</a></td>
    </tr>
  `
    )
    .join("");
}

function renderOptions(dict, selected = null) {
  return dict.map((d) => `<option value="${d.id}" ${selected == d.id ? "selected" : ""}>${d.name}</option>`).join("");
}

app.get("/", (req, res) => {
  const sql = `SELECT m.*, d.name AS dict_name FROM main m JOIN dict d ON m.dict_id = d.id`;

  db.all(sql, (err, rows) => {
    const table = renderRows(rows);
    res.send(render("index.html", { table }));
  });
});

app.get("/add", (req, res) => {
  db.all("SELECT * FROM dict", (err, dict) => {
    const options = renderOptions(dict);
    res.send(render("add.html", { options, error: "" }));
  });
});

app.get("/edit/:id", (req, res) => {
  db.get("SELECT * FROM main WHERE id=?", [req.params.id], (err, row) => {
    if (!row) return res.send("Not found");

    db.all("SELECT * FROM dict", (err, dict) => {
      const options = renderOptions(dict, row.dict_id);
      res.send(
        render("edit.html", {
          id: row.id,
          options,
          title: row.title,
          description: row.description,
          quantity: row.quantity,
          error: "",
        })
      );
    });
  });
});

app.get("/delete/:id", (req, res) => {
  db.get(
    "SELECT m.*, d.name AS dict_name FROM main m JOIN dict d ON m.dict_id=d.id WHERE m.id=?",
    [req.params.id],
    (err, row) => {
      if (!row) return res.send("Not found");
      res.send(
        render("delete.html", {
          id: row.id,
          title: row.title,
          category: row.dict_name,
        })
      );
    }
  );
});

app.post("/add", (req, res) => {
  const { dict_id, title, description, quantity } = req.body;
  let errors = [];

  if (!dict_id) errors.push("Choose a category.");
  if (!title.trim()) errors.push("Title required.");
  if (isNaN(quantity) || quantity < 0) errors.push("Quantity must be >= 0.");

  if (errors.length) {
    db.all("SELECT * FROM dict", (err, dict) => {
      const options = renderOptions(dict, dict_id);
      return res.send(
        render("add.html", {
          options,
          error: errors.join("<br>"),
        })
      );
    });
    return;
  }

  db.run(
    "INSERT INTO main (dict_id,title,description,quantity,created_at) VALUES (?,?,?,?,?)",
    [dict_id, title, description, quantity, new Date().toISOString()],
    () => res.redirect("/")
  );
});

app.listen(3000, () => console.log("Server running at http://localhost:3000"));
