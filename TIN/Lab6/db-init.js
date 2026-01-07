const sqlite3 = require("sqlite3").verbose();
const db = new sqlite3.Database("./lab6.db");

db.serialize(() => {
  db.run("DROP TABLE IF EXISTS main");
  db.run("DROP TABLE IF EXISTS dict");

  db.run(`
    CREATE TABLE dict (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      name TEXT NOT NULL
    )
  `);

  db.run(`
    CREATE TABLE main (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      dict_id INTEGER NOT NULL,
      title TEXT NOT NULL,
      description TEXT,
      quantity INTEGER NOT NULL DEFAULT 0,
      created_at TEXT NOT NULL,
      FOREIGN KEY(dict_id) REFERENCES dict(id)
    )
  `);

  const dict = ["A", "B", "C", "D", "E"];
  const stmt = db.prepare("INSERT INTO dict (name) VALUES (?)");
  dict.forEach((name) => stmt.run(name));
  stmt.finalize();

  db.run("INSERT INTO main (dict_id, title, description, quantity, created_at) VALUES (?,?,?,?,?)", [
    1,
    "Example item",
    "Sample text",
    10,
    new Date().toISOString(),
  ]);
});

db.close();
