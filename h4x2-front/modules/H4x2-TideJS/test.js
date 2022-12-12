fetch('https://h4x2-ork2.azurewebsites.net/isPublic')
  .then((response) => response.text())
  .then((data) => console.log(data));