window.saveAsFile = (filename, bytesBase64) => {
    const link = document.createElement('a');
    link.download = filename;
    link.href = bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
