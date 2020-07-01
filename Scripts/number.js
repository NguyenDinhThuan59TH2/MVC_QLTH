const readVietnamese = num => {
    if (num === 0) {
        return numArray[0]
    }
    let string = ''
    let hauto = ''
    let billion
    do {
        billion = num % 1000000000
        num = Math.floor(num / 1000000000)
        if (num > 0) {
            string = ReadMillions(billion, true) + hauto + string
        } else {
            string = ReadMillions(billion, false) + hauto + string
        }
        hauto = ' tỷ'
    } while (num > 0)
    return string
}