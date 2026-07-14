const fs = require('fs');
const pdf = require('pdf-parse');
const file = 'c:/Users/manit/Downloads/Airmaster Coding Evaluation Excercise.pdf';
fs.readFile(file, async (err, data) => {
  if (err) {
    console.error(err);
    process.exit(1);
  }
  try {
    const result = await pdf(data);
    console.log(result.text);
  } catch (e) {
    console.error(e);
    process.exit(1);
  }
});
