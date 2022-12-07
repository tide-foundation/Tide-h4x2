import NodeClient from "./Clients/NodeClient.js";
import Point from "./Ed25519/point.js";
import PrismFlow from "./Flow/PRISM.js";
import { decryptData, encryptData } from "./Tools/AES.js";
import { base64ToBytes } from "./Tools/Utils.js";

export function test(){ // point function testing
  var b = 'uG7uZxtPjT+YBruVT3D/vml8kAs1yi713Mos/DcTbmN0GtOpZo3G1jQBRTIQz8JV2sMB3XT343U+LAsWk9b0Mw==';
  var f = base64ToBytes(b);
}

export async function test2(){ // initial client testing
  var nodeclient = new NodeClient('http://h4x2-ork2.azurewebsites.net', 'Test');
  var applied = await nodeclient.Apply(Point.fromB64('uG7uZxtPjT+YBruVT3D/vml8kAs1yi713Mos/DcTbmN0GtOpZo3G1jQBRTIQz8JV2sMB3XT343U+LAsWk9b0Mw=='));
  console.log(applied.toBase64());
  return applied.toBase64();
}

export async function test3(){
  var enc = await encryptData("Chickens", "i2cxrwh+U+XGK6xOhqqbOHre2xTC6COAbwYBcuiOEW8D9tQSebQVSL932s7WTTPtXxPsvqx+fNJkvDBIPLSLFQ==");
  console.log(enc);
}

export async function test4(){ // test AES encryptions
  var pass = "Julio"
  var data = "Chickens"
  var encrypted = await encryptData(data, pass);

  console.log(encrypted)

  var decrypted = await decryptData(encrypted, pass);
  console.log(decrypted);
}

export async function test5(){ // get auth point to encrypt manually
  var config = {
    pass: "Julio",
    urls: ["http://h4x2-ork1.azurewebsites.net", "https://h4x2-ork2.azurewebsites.net"],
    encryptedData: "No Data"
  }

  const flow = new PrismFlow(config);
  const point = await flow.getAuthPoint();

  console.log(point);
}

export async function test6(){ // full test
  var config = {
    pass: "Julio",
    urls: ["http://h4x2-ork1.azurewebsites.net", "https://h4x2-ork2.azurewebsites.net"],
    encryptedData: "zn7826Oa2t8VmPq0g0t5Amdsifve7uXm1JU/5p5TeQcE+jQ4N50dCkJpiJq++6qJA0Uo1w=="
  }

  const flow = new PrismFlow(config);
  const decrypted = await flow.start();
  console.log(decrypted);
}